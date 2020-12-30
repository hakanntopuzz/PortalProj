using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Extensions;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DevPortal.Business.Services
{
    public class AuditService : IAuditService
    {
        #region ctor

        readonly IAuditRepository auditRepository;

        readonly ILoggingService loggingService;

        public AuditService(IAuditRepository auditRepository, ILoggingService loggingService)
        {
            this.auditRepository = auditRepository;
            this.loggingService = loggingService;
        }

        #endregion

        #region add audit

        public async Task<bool> AddAsync(AuditInfo auditInfo)
        {
            try
            {
                var types = auditInfo.OldRecord.GetType().GetProperties();

                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    foreach (var item in types)
                    {
                        if (item.PropertyType.IsGenericType)
                        {
                            continue;
                        }

                        if (IsRecordUpdateInfo(item))
                        {
                            continue;
                        }

                        var oldValue = GetOldValue(auditInfo.OldRecord, item);
                        var newValue = GetNewValue(auditInfo.NewRecord, item);

                        if (ValuesEmpty(oldValue, newValue))
                        {
                            continue;
                        }

                        if (IsRecordChanged(oldValue, newValue))
                        {
                            await AddAuditCoreAsync(auditInfo.TableName, auditInfo.RecordId, item.Name, oldValue, newValue, auditInfo.ModifiedBy);
                        }
                    }

                    scope.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(AddAsync), "Audit ekleme sırasında bir hata oluştu.", ex);

                return false;
            }
        }

        static object GetOldValue(object oldRecord, PropertyInfo item)
        {
            return item.GetValue(oldRecord, null);
        }

        static object GetNewValue(object newRecord, MemberInfo item)
        {
            return newRecord.GetType().GetProperty(item.Name).GetValue(newRecord, null);
        }

        static bool ValuesEmpty(object oldValue, object newValue)
        {
            return oldValue == null && newValue == null;
        }

        static bool IsRecordChanged(object oldValue, object newValue)
        {
            return IsOldValueNullAndNewValueIsNotNull(oldValue, newValue)
                || IsOldValueIsNotNullAndNewValueIsNull(oldValue, newValue)
                || !IsOldValueAndNewValueSame(oldValue, newValue);
        }

        static bool IsOldValueNullAndNewValueIsNotNull(object oldValue, object newValue)
        {
            return oldValue == null && newValue != null;
        }

        static bool IsOldValueIsNotNullAndNewValueIsNull(object oldValue, object newValue)
        {
            return oldValue != null && newValue == null;
        }

        static bool IsOldValueAndNewValueSame(object oldValue, object newValue)
        {
            return oldValue.Equals(newValue);
        }

        async Task AddAuditCoreAsync(string tableName, int recordId, string fieldName, object oldRecord, object newRecord, int modifiedBy)
        {
            var oldRecordValue = GetValueAsString(oldRecord);
            var newRecordValue = GetValueAsString(newRecord);

            var isSuccess = await auditRepository.AddAsync(tableName, recordId, fieldName, oldRecordValue, newRecordValue, modifiedBy);

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        #endregion

        static string GetValueAsString(object value)
        {
            return value == null
                ? null
                : value.ToString();
        }

        #region get audit list

        public async Task<JQTable> GetFilteredAuditListAsJqTableAsync(AuditTableParam tableParam)
        {
            if (tableParam == null)
            {
                return null;
            }

            string sortDirection = SetAuditFilterSortDirection(tableParam);

            var data = await auditRepository.GetFilteredAuditListAsync(
                tableParam.start,
                tableParam.length,
                tableParam.SortColumn,
                sortDirection,
                tableParam.SearchText,
                tableParam.TableName,
                tableParam.RecordId);

            int recordCount = SetAuditListRecordCount(data);

            return new JQTable
            {
                data = data,
                recordsFiltered = recordCount,
                recordsTotal = recordCount
            };
        }

        static string SetAuditFilterSortDirection(TableParam tableParam)
        {
            if (tableParam.order.Any())
            {
                return tableParam.order.First().dir;
            }

            return null;
        }

        static int SetAuditListRecordCount(ICollection<Audit> data)
        {
            if (data.Any())
            {
                return data.First().TotalCount;
            }

            return 0;
        }

        #endregion

        #region is changed

        public bool IsChanged(object oldRecord, object newRecord, string compareModelName)
        {
            var type = Type.GetType($"DevPortal.Model.{ compareModelName }, DevPortal.Model");

            var properties = type.GetProperties();

            foreach (var item in properties)
            {
                if (IsRecordUpdateInfo(item))
                {
                    continue;
                }

                var oldValue = oldRecord.GetValue(item.Name);
                var newValue = newRecord.GetValue(item.Name);

                if (BothValueIsNull(oldValue, newValue))
                {
                    continue;
                }

                if (OneValueIsNull(oldValue, newValue))
                {
                    return true;
                }

                if (!oldValue.Equals(newValue))
                {
                    return true;
                }
            }

            return false;
        }

        static bool IsRecordUpdateInfo(PropertyInfo property)
        {
            return property.PropertyType.Name == nameof(RecordUpdateInfo);
        }

        static bool BothValueIsNull(object oldValue, object newValue)
        {
            return oldValue == null && newValue == null;
        }

        static bool OneValueIsNull(object oldValue, object newValue)
        {
            return (oldValue == null && newValue != null) || (oldValue != null && newValue == null);
        }

        #endregion
    }
}