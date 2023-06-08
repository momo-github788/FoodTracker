<template>
    <div class="food-records-container mt-4">

        <div class="metadata">
        <router-link :to="{name: 'add'}">
            <button class="btn btn-primary mt-4 add-btn">Add Food item</button>
        </router-link>

        <SearchBarComponent @onHandleChange="logChange"/>
        <SortComponent/>
        </div>

   
        <div class="food-record-container" v-for="foodRecord in foodRecords" :key="foodRecord.id">
            <FoodRecordItem :foodRecord="foodRecord" @onDeleteClick="handleDelete(foodRecord.id)"/>
        </div>
     

    </div>
</template>

<script>
import useFoodRecords from '@/composables/FoodRecord/foodRecord';
import FoodRecordItem from '../components/FoodRecordItem.vue'
import SortComponent from '../components/SortComponent.vue'
import SearchBarComponent from '../components/SearchBarComponent.vue'
import { onMounted, onUpdated, ref, watchEffect } from 'vue';

    export default {
        components: {
        SearchBarComponent, FoodRecordItem, SortComponent,
        },
        props: [],
        setup() {

           const { deleteFoodRecord, getFoodRecords, foodRecords, searchFoodRecords} = useFoodRecords()
          
           getFoodRecords()

        
        
            const logChange = (query) => {
                console.log(query)
            }
    
            

            const length = ref(null);

            onUpdated(() => {
     
                length.value = foodRecords.value.length;
            })

            const handleDelete = (id) => {
        
                if (!window.confirm("Are sure?")) {
                    return;
                }
               deleteFoodRecord(id)
             
            }

            

            return {
                length, foodRecords, logChange, handleDelete
            }
   
        }
    }

</script>

<style scoped>

.food-records-container {

}
.food-records-container  .add-btn {

    padding: 12px;
    font-weight: 600;
    margin: 1rem 1rem 1rem 0;
}


.food-records-container .update-btn {

}

.metadata {

    flex: 3;

}
</style>