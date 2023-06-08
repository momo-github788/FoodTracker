<template>
    <div class="update-food-container" v-if="foodRecord">
        <h1>Update a Food item</h1>

        <form class="mt-5" @submit.prevent="handleUpdate">
            <div class="mb-3 mt-3">
                <label class="form-label">Name:</label>
                <input type="text" class="form-control" placeholder="Enter name" v-model="foodRecord.name">
            </div>
            <div class="mb-3">
                <label class="form-label">Value:</label>
                <input type="text" class="form-control" placeholder="Enter value" v-model="foodRecord.value">
            </div>

            <div class="mb-3">
                <label class="form-label">Food Category:</label>
                <input type="text" class="form-control" placeholder="Enter value" v-model="foodRecord.foodCategory">
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
        </form>
    </div>
</template>

<script>
import useFoodRecords from '@/composables/FoodRecord/foodRecord.js';
import ApiService from '@/services/ApiService';
import router from '@/router';
import { onMounted } from 'vue';


export default {
    props : {
      id: {
        required: true,
        type: String,
      },
    },
    setup(props) {
        
        const {getFoodRecord, foodRecord, errors, updateFoodRecord} = useFoodRecords()

        console.log("Props id: ", props.id)
        onMounted(() => {
            getFoodRecord(props.id)
        })

        const handleUpdate = () => {
            updateFoodRecord(props.id)
        }

        return {
            foodRecord, errors, handleUpdate
        }

    
    }
}
</script>

<style>
form {
 
    text-align: left;
    width: 550px;
    border-radius: 10px;
    margin: 0 auto;
    padding: 40px;
    background-color: rgb(255, 255, 255);
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.1), 
    0 6px 20px 0 rgba(0, 0, 0, 0.19);
}

form button {
    width: 100%;
}

form .div {
    width: 100%;
}
</style>