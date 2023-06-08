<template>
  <div class="home">
    <h1>{{ title }}</h1>
    <h4>Take care of your daily calories intake</h4>

    <router-view></router-view>
    <FoodRecords :foodRecords="foodRecords"/>


  </div>
</template>

<script>
// @ is an alias to /src

import { onMounted, ref } from 'vue';
import FoodRecords from '../components/FoodRecords.vue'
import  useFoodRecords from '../composables/FoodRecord/foodRecord.js'


export default {
  components: {
    FoodRecords
  },
  setup() {
    const title = ref("Food Tracker Application")

    const {getFoodRecords, foodRecords} = useFoodRecords()

    onMounted(() => {
      getFoodRecords()
    })

    return {
      title, foodRecords
    }
  }
}
</script>

<style scoped>




</style>
