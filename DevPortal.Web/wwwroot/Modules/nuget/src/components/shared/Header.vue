<template>
  <header>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <div class="container-middle d-xl-flex d-lg-flex justify-content-between align-items-center position-relative">
          <a class="navbar-brand" href="/"><img src="/img/devportal-logo.png" alt="dev portal logo" width="80" /></a>
          <div class="nav-item-profile dropdown float-right">
              <a class="nav-link dropdown-toggle" href="#" id="profileDropdown" role="button" data-toggle="dropdown"
                aria-haspopup="true" aria-expanded="false">{{ userIdentityName }}</a>
              <div class="dropdown-menu dropdown-menu-right" aria-labelledby="profileDropdown">
                  <template v-for="item in userProfile">
                    <a v-for="child in item.children" :key="child.name" class="dropdown-item" :href="child.link">
                      <i v-if="child.icon !== null && child.icon !== ''" :class="'fa ' + child.icon" aria-hidden="true"></i>
                      {{ child.name }}
                    </a>
                  </template>
              </div>
          </div>
          <button class="navbar-toggler float-right" type="button" data-toggle="collapse" data-target="#navbarSupportedContent"
                  aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
              <span class="navbar-toggler-icon"></span>
          </button>
          <div class="collapse navbar-collapse justify-content-end" id="navbarSupportedContent">
              <ul class="nav navbar-nav">
                <li class="nav-item dropdown" v-for="item in mainMenu" :key="item.name">
                    <a class="nav-link dropdown-toggle" href="#" id="appsDropdown" role="button" data-toggle="dropdown"
                      aria-haspopup="true" aria-expanded="false">
                        {{ item.name }}
                    </a>
                    <div class="dropdown-menu" aria-labelledby="appsDropdown" v-if="item.children.length > 0">
                      <a class="dropdown-item" :href="child.link" v-for="child in item.children" :key="child.name">{{ child.name }}</a>
                    </div>
                </li>
              </ul>
          </div>
      </div>
    </nav>
  </header>
</template>
<script>
export default {
  data () {
    return {
      mainMenu: [],
      userProfile: [],
      userIdentityName: ''
    }
  },
  created () {
    this.$store.dispatch('getMenu').then(menu => {
      this.mainMenu = menu.filter(x => x.group === 1)
      this.userProfile = menu.filter(x => x.group === 2)
    })

    this.$store.dispatch('getUserIdentityName').then(response => {
      this.userIdentityName = response
    })
  }
}
</script>
<style>
  .dropdown-item {
    padding: 1.25rem 1.5rem;
  }

  .bg-light {
    background-color: #f8f9fa !important;
  }

  .nav-link:hover, .dropdown.show .nav-link, .nav-item.active .nav-link {
    color: #4667dd !important;
  }

  @media (max-width:768px) {
    .dropdown-item {
        padding: .5rem 1rem;
        color: rgba(0,0,0,.5);
    }
  }

  .nav-item-profile {
    color: #fff;
    width: 40px;
    height: 40px;
    background: #f2695c;
    border-radius: 50%;
    text-align: center;
    font-weight: 600;
  }
</style>
