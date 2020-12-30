<template>
  <div id="app">
    <Header />
    <b-container class="container-middle">
      <nav class="m-2">
        <ol class="breadcrumb">
          <template v-for="item in breadcrumbs">
            <li v-if="item.link === null" :key="item.name" class="breadcrumb-item">{{ item.name }}</li>
            <router-link v-if="item.name && item.link !== null" :key="item.name" class="breadcrumb-item" tag="li" :to="item.link"><a @click="reloadPage">{{ item.name }}</a></router-link>
          </template>
        </ol>
      </nav>
      <router-view/>
    </b-container>
    <Footer />
  </div>
</template>
<script>
import Header from '../src/components/shared/Header'
import Footer from '../src/components/shared/Footer'

export default {
  data () {
    return {
      breadcrumbs: []
    }
  },
  created () {
    this.setBreadcrumbs()
  },
  methods: {
    setBreadcrumbs () {
      this.breadcrumbs = this.$route.meta.breadcrumb(this.$route)
    },
    reloadPage (event) {
      var pathName = event.target.pathname.split('/nuget')[1]
      if (pathName === this.$route.path) {
        location.reload(true)
      }
    }
  },
  components: {
    Header,
    Footer
  },
  watch: {
    '$route' () {
      this.setBreadcrumbs()
    }
  }
}
</script>
<style>
  html {
    min-height: 100%;
    position: relative;
  }

  html, body {
    color: #242c39;
    font-size: 16px;
    box-sizing: border-box;
  }

  body {
    /* Margin bottom by footer height */
    margin-bottom: 60px;
  }

  .container-middle {
    width: 100%;
    padding-right: 15px;
    padding-left: 15px;
    margin-right: auto;
    margin-left: auto;
  }

  @media (min-width: 576px) {
      .container-middle {
          max-width: 540px;
      }
  }

  @media (min-width: 768px) {
      .container-middle {
          max-width: 720px;
      }
  }

  @media (min-width: 992px) {
      .container-middle {
          max-width: 960px;
      }
  }

  @media (min-width: 1200px) {
      .container-middle {
          max-width: 1300px;
      }
  }

  .breadcrumb {
    display: flex;
    -ms-flex-wrap: wrap;
    flex-wrap: wrap;
    padding: .75rem 1rem;
    margin-bottom: 1rem;
    list-style: none;
    background-color: #e9ecef;
    border-radius: .25rem;
  }
</style>
