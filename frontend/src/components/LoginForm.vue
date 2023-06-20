<template>
  <form @submit.prevent="handleSubmit">

    <div class="mb-3">
      <label class="form-label">Username</label>
      <input type="text" v-model="request.userName" class="form-control" placeholder="Enter username"/>
    </div>

    <div class="mb-3">
      <label class="form-label">Password</label>
      <input type="password" v-model="request.password" class="form-control" placeholder="Enter password"/>
    </div>


    <button type="submit" class="btn btn-primary">Submit</button>
  </form>
</template>

<script>
import { computed, onMounted, reactive, ref } from "vue";
import useAuth from "@/composables/FoodRecord/auth.js";


export default {
  components: {},
  setup() {
    const { login, errors, getCurrentUserAccessToken} = useAuth();


    const request = ref({
      userName: "admin1234",
      password: "P@ssw0rd",
    })

 


    // const validateForm = () => {
    
    //  return true;
    // };


    const handleSubmit = async () => {
    
        await login(request.value);
        console.log(errors.value)
       
    };

    const hasErrors = computed(() => Object.keys(errors.value).length > 0)

    return {
      errors,
      request,
      handleSubmit,
      hasErrors
      
    };
  },
};
</script>

<style></style>
