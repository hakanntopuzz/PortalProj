using DevPortal.Data.Abstract;
using DevPortal.JenkinsManager.Model;
using System;

namespace DevPortal.Data.Factories
{
    public class JenkinsFactory : IJenkinsFactory
    {
        public JenkinsJobItem CreateJenkinsJobItem(string name, Uri uriString, string color)
        {
            return new JenkinsJobItem
            {
                Name = name,
                Url = uriString,
                Color = color
            };
        }
    }
}