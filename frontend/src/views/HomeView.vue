<template>
  <div class="home mt-5">
    <h1>{{ title }}</h1>
    <h3>Take care of your daily calories intake</h3>

    <FoodRecords/>
  </div>
</template>

<script>
// @ is an alias to /src

import { ref } from 'vue';
import FoodRecords from '../components/FoodRecords.vue'

export default {

  components: {
    FoodRecords 
  },
  setup() {
    const title = ref("Food Tracker Application")

    return {
      title
    }
  }
}
</script>

<style scoped>

.home {


}
h1, h2 {
  font-weight: normal;

}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #35495E;
}
</style>
