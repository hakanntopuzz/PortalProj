<template>
  <main role="main" class="py-4">
    <loading :active.sync="isLoading" :can-cancel="true" :is-full-page="fullPage" :loader="loaderType"></loading>
    <h1 class="title-primary display-4">NuGet Paketleri</h1>
    <b-row>
      <b-col md="12"  v-if="Isfavourite">
        <b-button variant="btn btn-simple btn-sm hover-action float-right active"  id="deleteFavourite" @click="DeleteFavourites()">
        <i class="fa fa-fa fa-star" aria-hidden="true"></i> Favorilerimden Çıkar
      </b-button>
      </b-col>
      <b-col md="12" v-else>
      <b-button variant="btn btn-simple btn-sm hover-action float-right" id="addFavourite" @click="AddFavourites()">
        <i class="fa fa-fa fa-star" aria-hidden="true"></i> Favorilerime Ekle
      </b-button>
      </b-col>
    </b-row>
    <b-row align-v="center" class="mt-3">
      <b-col md="3" offset-md="5" sm="6" class="form-group">
        <b-input-group size="md">
          <b-input-group-prepend is-text>
            <b-icon icon="search"></b-icon>
          </b-input-group-prepend>
          <b-form-input type="search" placeholder="Paket ara" v-model="searchText" @input="onSearch"></b-form-input>
        </b-input-group>
      </b-col>
      <b-col md="2" sm="6" class="form-group">
        <b-input-group size="md">
          <b-form-select v-model="sortModel.selected" :options="sortModel.options" @change="onSort"></b-form-select>
        </b-input-group>
      </b-col>
      <b-col md="2" sm="6" class="form-group">
          <b-dropdown id="dropdown-1" text="Diğer İşlemler" class="btn-block">
            <b-dropdown-item @click="exportPackageList(filteredPackages)">Dışa Aktar</b-dropdown-item>
          </b-dropdown>
      </b-col>
    </b-row>
    <b-row>
      <b-col md="12">
        <h2 class="no-package text-center" v-if="packages.length === 0">Paket bulunamadı</h2>
        <b-card v-for="item in filteredPackages" :key="item.id">
          <b-media>
            <template v-slot:aside>
              <b-img v-if="item.properties.iconUrl !== null && item.properties.iconUrl !== ''" blank-color="#ccc" width="64" alt="placeholder" :src="item.properties.iconUrl"></b-img>
              <b-img v-else blank-color="#ccc" width="64" alt="placeholder" src="https://www.nuget.org/Content/gallery/img/default-package-icon.svg"></b-img>
            </template>
            <h3 class="package-title">
              <router-link :to="'packages/' + item.id">{{ item.id }}</router-link>
            </h3>
            <div class="package-detail-div">
              <span class="package-span"><b-icon-clock></b-icon-clock> Son Güncellenme Tarihi: {{ new Date(item.properties.published) | moment('DD-MM-YYYY hh:mm') }}</span>
              <div class="seperator">|</div>
              <span class="package-span"><b-icon-flag></b-icon-flag> Son Versiyon: {{ item.properties.version }}</span>
              <div class="seperator">|</div>
              <span class="package-span"><b-icon-tag></b-icon-tag> Etiketler: {{ item.properties.tags }}</span>
              <p> {{ item.properties.description }} </p>
            </div>
          </b-media>
        </b-card>
      </b-col>
      <b-col md="6" class="table-footer">
        <label class="text-left">Toplam {{ totalCount }} kayıttan {{ packages.length }} tanesi listeleniyor.</label>
      </b-col>
      <b-col md="3" class="table-footer">
        <b-pagination
          v-model="pagingModel.currentPage"
          :total-rows="totalCount"
          :per-page="pagingModel.take"
          first-number
          last-number
          align="right"
          @change="onPaging"
        ></b-pagination>
      </b-col>
      <b-col md="3" class="table-footer">
        <b-input-group size="md" prepend="Kayıt Sayısı">
          <b-form-select v-model="pagingModel.take" :options="pagingModel.takeOptions" @change="onPerPage"></b-form-select>
        </b-input-group>
      </b-col>
    </b-row>
  </main>
</template>

<script>
import Loading from 'vue-loading-overlay'
import axios from 'axios'

export default {
  data () {
    return {
      pagingModel: {
        currentPage: 1,
        pages: 1,
        skip: 0,
        take: 5,
        takeOptions: [
          { value: 5, text: '5 (Varsayılan)' },
          { value: 10, text: '10' },
          { value: 25, text: '25' },
          { value: 50, text: '50' }
        ]
      },
      sortModel: {
        selected: 0,
        options: [
          { value: 0, text: 'Sıralama (Varsayılan)' },
          { value: 1, text: 'En eskiler' },
          { value: 2, text: 'En yeniler' }
        ]
      },
      searchText: '',
      packages: [],
      filteredPackages: [],
      totalCount: 0,
      isLoading: true,
      fullPage: true,
      loaderType: 'dots',
      timeOut: null,
      errorMessage: 'İndirme işlemi gerçekleştirilemedi.',
      successMessage: 'İndirme işlemi başarılı',
      favouritePageName: 'NuGet Paketleri',
      favouriteId: 0,
      Isfavourite: false
    }
  },
  components: {
    Loading
  },
  created () {
    this.favoriteSearch()
    this.getPackages()
  },
  methods: {
    getPackages () {
      let payload = {
        skip: this.pagingModel.take * (this.pagingModel.currentPage - 1),
        take: this.pagingModel.take,
        searchString: this.searchText,
        orderBy: this.sortModel.selected
      }
      this.$store.dispatch('getNugetPackages', payload).then(data => {
        this.isLoading = true
        this.totalCount = data.totalCount
        this.packages = data.nugetPackages
        this.filteredPackages = this.packages
      }).finally(() => {
        this.isLoading = false
      })
    },
    onPaging (val) {
      this.pagingModel.currentPage = val
      this.getPackages()
    },
    onSearch (val) {
      if (this.timeOut) {
        clearTimeout(this.timeOut)
      }
      this.timeOut = setTimeout(() => {
        this.getPackages()
      }, 500)
    },
    onSort () {
      this.pagingModel.currentPage = 1
      this.getPackages()
    },
    onPerPage () {
      this.pagingModel.currentPage = 1
      this.getPackages()
    },
    base64ToArrayBuffer (base64) {
      var binaryString = window.atob(base64)
      var binaryLen = binaryString.length
      var bytes = new Uint8Array(binaryLen)
      for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i)
        bytes[i] = ascii
      }
      return bytes
    },
    saveByteArray (fileName, byte) {
      var blob = new Blob([byte])
      var link = document.createElement('a')
      link.href = window.URL.createObjectURL(blob)
      link.download = fileName
      link.click()
    },
    exportPackageList () {
      axios.get(`export-nuget-packages?searchString=${this.searchText}&totalCount=${this.totalCount}`).then(response => {
        console.log(response.data)
        if (response.data != null) {
          var sampleArr = this.base64ToArrayBuffer(response.data)
          this.saveByteArray('nuget-paketleri.csv', sampleArr)
          this.makeToast('BİLGi', 'success', this.successMessage)
        }
      }).catch(error => {
        console.log(error)
        this.makeToast('UYARI', 'danger', this.errorMessage)
        this.isLoading = false
      }).finally(() => {
        this.isLoading = false
      })
    },
    makeToast (title, variant, message) {
      this.$bvToast.toast(message, {
        title: ` ${title || 'UYARI'}`,
        variant: variant,
        solid: true
      })
    },
    favoriteSearch () {
      var pageUrl = window.location.pathname + window.location.search
      axios.get(`check-page-is-favourites?pageUrl=${pageUrl}`).then(response => {
        if (response.data.isSuccess === true) {
          this.favouriteId = response.data.data.id
          this.Isfavourite = true
        } else {
          this.Isfavourite = false
        }
      }).catch(error => {
        this.makeToast('HATA', 'danger', error)
      }).finally(() => {
        this.isLoading = false
      })
    },
    addFavorite (favouritePageName, pageUrl) {
      let addToFavouritesRequest = {
        PageTitle: favouritePageName,
        PageUrl: pageUrl
      }
      axios.post('add-to-favourites', addToFavouritesRequest).then(response => {
        if (response.data.isSuccess === true) {
          this.$swal(' ', response.data.message, 'success')
          this.favoriteSearch()
        } else {
          this.$swal(' ', response.data.message, 'danger')
        }
      }).catch(error => {
        this.makeToast('HATA', 'danger', error)
      }).finally(() => {
        this.isLoading = false
      })
    },
    deleteFavorite (id) {
      let removeFromFavouritesRequest = {
        Id: id
      }
      axios.post('remove-from-favourites', removeFromFavouritesRequest).then(response => {
        if (response.data.isSuccess === true) {
          this.$swal(' ', response.data.message, 'success')
          this.favoriteSearch()
        } else {
          this.$swal(' ', response.data.message, 'danger')
        }
      }).catch(error => {
        this.makeToast('HATA', 'danger', error)
      }).finally(() => {
        this.isLoading = false
      })
    },
    AddFavourites () {
      var pageUrl = window.location.pathname + window.location.search
      this.addFavorite(this.favouritePageName, pageUrl)
    },
    DeleteFavourites () {
      var id = this.favouriteId
      this.deleteFavorite(id)
    }
  }
}
</script>

<style scoped>
.jumbotron {
    padding: 1rem 1rem !important;
}
.package-count {
  font-weight: 300;
}
.card {
  border: initial;
}
.package-title {
  font-weight: 300;
}
.package-title a {
  word-break: break-all;
}
.package-detail-div {
  color: grey;
}
.package-detail-div p {
  color: #000000;
  font-weight: 500;
}
.seperator {
  padding-right: 15px;
  padding-left: 15px;
  display: inline-block;
}
.container {
  margin-bottom: 30px;
}
.no-package {
  margin-top: 45px;
  margin-bottom: 45px;
}
@media (max-width: 768px) {
  .table-footer, .table-header {
    margin-top: 10px;
  }
}
</style>
