<template>
    <div class="container-fluid food-records mt-4">

       
        <router-link :to="{name: 'add'}">
            <button class="btn btn-primary mt-4 add-btn">Add Food item</button>
        </router-link>

         Length: {{length}}
        <table class="table mt-3">
            <thead class="table-dark">
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Value</th>
                    <th>Date Time</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="record in foodRecords" :key="record.id">
                    <td>{{ record.id }}</td>
                    <td>{{ record.name }}</td>
                    <td>{{ record.value }}</td>
                    <td>{{ record.dateTime }}</td>
                    <td>

                        
                        <router-link :to="{name: 'update', params: {id: record.id}}">
                            <button class="btn btn-secondary update-btn">Update</button>
                        </router-link>

                 
                        <button class="btn btn-danger delete-btn" @click="handleDelete(record.id)">Delete</button>
                       
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
</template>

<script>
import { onMounted, reactive, ref } from 'vue';
import ApiService from '@/services/ApiService';
import useFoodRecords from '@/composables/FoodRecord/foodRecord';

    export default {
        props: ['foodRecords'],
        setup() {
            const { deleteFoodRecord, getFoodRecords, foodRecords} = useFoodRecords()
            const length = ref(foodRecords.length)
            const handleDelete = (id) => {
                if (!window.confirm("You sure?")) {
                    return;
                }
               deleteFoodRecord(id)
               getFoodRecords()
            }

            return {
                handleDelete, length, foodRecords
            }
   
        }
    }

</script>

<style scoped>

.food-records {
    max-width: 1000px;
}
.container-fluid .add-btn {
    float: left;
    padding: 12px;
    font-weight: 600;
    margin: 1rem 1rem 1rem 0;
}


.container-fluid .update-btn {

}
</style>