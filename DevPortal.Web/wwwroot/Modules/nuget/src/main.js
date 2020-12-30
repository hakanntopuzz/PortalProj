import Vue from 'vue'
import axios from 'axios'
import moment from 'vue-moment'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import VueLoading from 'vue-loading-overlay'
import LoadScript from 'vue-plugin-load-script'
import VueSweetalert2 from 'vue-sweetalert2'

import App from './App.vue'
import router from './router'
import store from './store'

import 'bootstrap-vue/dist/bootstrap-vue.css'
import 'vue-loading-overlay/dist/vue-loading.css'
import '../../../bundles/css/main.min.css'
import '../../../css/theme.css'
import 'sweetalert2/dist/sweetalert2.min.css'

Vue.config.productionTip = false

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.use(moment)
Vue.use(VueLoading)
Vue.use(LoadScript)
Vue.use(VueSweetalert2)

if (location.hostname !== 'localhost') {
  Vue.loadScript('../../../bundles/js/main.min.js')
} else {
  Vue.loadScript('/assets/js/main.min.js')
}

axios.defaults.baseURL = process.env.VUE_APP_APIURL

axios.interceptors.response.use(function (response) {
  return response
}, function (error) {
  if (error && error.response && error.response.status === 401) {
    location.href = location.origin + '/Account/Login?ReturnUrl=' + location.pathname
  }
  return error.response
})

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
