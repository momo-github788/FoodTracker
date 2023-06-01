<template>
  <div class="home mt-4">
    <h1>{{ title }}</h1>
    <h4>Take care of your daily calories intake</h4>

    <router-view></router-view>
    <FoodRecords :foodRecords="foodRecords"/>


  </div>
</template>

<script>
// @ is an alias to /src

import { ref } from 'vue';
import FoodRecords from '../components/FoodRecords.vue'
import UpsertFood from '@/components/UpsertFood.vue';
import getFoodRecords from '../composables/getFoodRecords.js'


export default {

  components: {
    FoodRecords,
    UpsertFood
},
  setup() {
    const title = ref("Food Tracker Application")

    const {foodRecords, error, load} = getFoodRecords()


    load()

    return {
      title, foodRecords, error
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
