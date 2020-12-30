using DevPortal.JenkinsManager.Model;
using System;

namespace DevPortal.Data.Abstract
{
    public interface IJenkinsFactory
    {
        JenkinsJobItem CreateJenkinsJobItem(string name, Uri uriString, string color);
    }
}