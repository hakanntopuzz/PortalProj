import Vue from 'vue'
import VueRouter from 'vue-router'

import Nuget from '../views/Home.vue'
import NugetDetail from '../views/NugetDetail.vue'
import NugetArchive from '../views/NugetArchive.vue'
import NuspecTemplate from '../views/NuspecTemplate.vue'
import OldNugetPackages from '../views/OldNugetPackages.vue'
import UploadNewNugetPackage from '../views/UploadNewNugetPackage.vue'
import UploadNewNugetPackageResult from '../views/UploadNewNugetPackageResult.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/packages',
    name: 'Nuget',
    component: Nuget,
    meta: {
      breadcrumb: route => [
        { name: 'NuGet', link: null },
        { name: 'Paketler', link: '/packages' }
      ]
    }
  },
  {
    path: '/archive',
    name: 'Archive',
    component: NugetArchive,
    meta: {
      breadcrumb: route => [
        { name: 'NuGet', link: null },
        { name: 'Arşiv', link: '/archive' }
      ]
    }
  },
  {
    path: '/nuspecTemplate',
    name: 'NuspecTemplate',
    component: NuspecTemplate,
    meta: {
      breadcrumb: route => [
        { name: 'NuGet', link: null },
        { name: 'Nuspec Şablonu', link: '/nuspectemplate' }
      ]
    }
  },
  {
    path: '/oldNugetPackages',
    name: 'OldNugetPackages',
    component: OldNugetPackages,
    meta: {
      breadcrumb: route => [
        { name: 'NuGet', link: null },
        { name: 'Emekli Nuget Paketleri', link: '/oldNugetPackages' }
      ]
    }
  },
  {
    path: '/uploadNewNugetPackage',
    name: 'UploadNewNugetPackage',
    component: UploadNewNugetPackage,
    meta: {
      breadcrumb: route => [
        { name: 'NuGet', link: null },
        { name: 'Nuget Paketi Yükleme', link: '/uploadNewNugetPackage' }
      ]
    }
  },
  {
    path: '/uploadNewNugetPackageResult',
    name: 'UploadNewNugetPackageResult',
    component: UploadNewNugetPackageResult,
    meta: {
      breadcrumb: route => [
        { name: 'NuGet', link: null },
        { name: 'Nuget Paketi Yükleme Sonucu', link: '/uploadNewNugetPackageResult' }
      ]
    }
  },
  {
    path: '/packages/:id/:version?',
    name: 'NugetDetail',
    component: NugetDetail,
    meta: {
      breadcrumb: route => [
        { name: 'NuGet', link: null },
        { name: 'Paketler', link: '/packages' },
        { name: route.params.id, link: `/packages/${route.params.id}` },
        { name: route.params.version, link: `/packages/${route.params.id}/${route.params.version}` }
      ]
    }
  },
  { path: '*', redirect: '/packages' }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

router.beforeEach((to, from, next) => {
  next()
})

export default router
