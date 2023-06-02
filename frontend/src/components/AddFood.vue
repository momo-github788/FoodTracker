<template>

    <h1>Add a Food item</h1>

    <div v-if="errors">
        {{errors[0]}}
    </div>

    <form class="mt-5" @submit.prevent="handleAddSubmit">
        <div class="mb-3 mt-3">
            <label class="form-label">Name:</label>
            <input type="text" class="form-control" placeholder="Enter name" v-model="foodRecordRequest.name">
        </div>
        <div class="mb-3">
            <label class="form-label">Value:</label>
            <input type="text" class="form-control" placeholder="Enter value" v-model="foodRecordRequest.value">
        </div>
        <button type="submit" class="btn btn-primary">Add</button>
    </form>

    {{name}}
    {{value}}
</template>

<script>
import { reactive, ref } from 'vue';
import useFoodRecords from '@/composables/FoodRecord/foodRecord';

export default {
  setup() {
    
    const foodRecordRequest = reactive({
      name: "",
      value: "",
      dateTime: "2023-06-02T11:25:58.0502344",
    });
  
    const {createFoodRecord, errors} = useFoodRecords();

    const handleAddSubmit = () => {
        createFoodRecord(foodRecordRequest);
        console.log("ERRORS: ", errors.value)

    };

    return {
      foodRecordRequest,
      handleAddSubmit,
      errors
    };
  },
};
</script>

<style>
</style>