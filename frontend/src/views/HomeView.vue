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
