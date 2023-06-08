<template>
    <div class="food-records-container mt-4">

        <div class="metadata">
            <router-link :to="{name: 'add'}">
                <button class="btn btn-primary mt-4 add-btn">Add Food item</button>
            </router-link>

            <SearchBarComponent @onHandleChange="filterResults"/>
            <SortComponent/>
        </div>

        <div v-if="foodRecords">
            <div class="food-record-container" v-for="foodRecord in foodRecords" :key="foodRecord.id">
                <FoodRecordItem :foodRecord="foodRecord" @onDeleteClick="handleDelete(foodRecord.id)"/>
            </div>
        </div>
   
        <div v-else>
            <h3>There are no matching results...</h3>
        </div>
     
        <Pagination />
    </div>
</template>

<script>
import useFoodRecords from '@/composables/FoodRecord/foodRecord';
import FoodRecordItem from '../components/FoodRecordItem.vue'
import Pagination from '../components/Pagination.vue'
import SortComponent from '../components/SortComponent.vue'
import SearchBarComponent from '../components/SearchBarComponent.vue'
import { onMounted, onUpdated, ref, watchEffect } from 'vue';

    export default {
        components: {
        SearchBarComponent, FoodRecordItem, SortComponent, Pagination
        },
        props: [],
        setup() {

           const { deleteFoodRecord, getFoodRecords, foodRecords} = useFoodRecords()
        

            getFoodRecords()

            const filterResults = (query) => {
                if(query === "") {
                    getFoodRecords()
                }
                getFoodRecords({
                    searchQuery: query
                })
            }


            const handleDelete = (id) => {
        
                if (!window.confirm("Are sure?")) {
                    return;
                }
               deleteFoodRecord(id)
             
            }

            

            return {
                length, foodRecords, filterResults, handleDelete
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