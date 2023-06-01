<template>
  <NavBar/>
  <router-view></router-view>


</template>

<script>
import NavBar from './components/NavBar.vue';
import HomeView from './views/HomeView.vue';

export default {
  components: {
    NavBar,
    HomeView
  },
  setup() {



    return {
      
    }
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

</style>
