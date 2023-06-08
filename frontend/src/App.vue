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
  font-family: "Source Sans Pro", "Helvetica Neue", Arial, sans-serif;
  text-rendering: optimizelegibility;
  -moz-osx-font-smoothing: grayscale;
  -moz-text-size-adjust: none;
  margin: 0;
  padding: 0;
  font-weight: 600;
}

p{
  margin: 0;
  padding: 0;
}

.home, .add-food-container, .update-food-container {
  width: 60%;
  margin: 3rem 0 0 5rem;


}

</style>
