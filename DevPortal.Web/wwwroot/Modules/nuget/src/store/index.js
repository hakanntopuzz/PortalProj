import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
  },
  mutations: {
  },
  actions: {
    getMenu ({ commit }) {
      return new Promise((resolve, reject) => {
        axios.get('/get-menu').then(response => {
          resolve(response.data)
        }).catch(error => {
          reject(error)
        })
      })
    },
    getNugetPackages ({ commit }, payload) {
      return new Promise((resolve, reject) => {
        console.log(payload.skip, payload.take, payload.searchString, payload.orderBy)
        axios.get(`/get-filtered-nuget-packages?skip=${payload.skip}&take=${payload.take}&searchString=${payload.searchString}&orderBy=${payload.orderBy}`).then(response => {
          resolve(response.data)
        }).catch((err) => {
          console.log(err)
          reject(err)
        })
      })
    },
    getUserIdentityName ({ commit }) {
      return new Promise((resolve, reject) => {
        axios.get('/account/get-user-identity-name').then(response => {
          resolve(response.data)
        }).catch(error => {
          reject(error)
        })
      })
    }
  },
  modules: {
  }
})
