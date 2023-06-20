<template>
  <form @submit.prevent="handleSubmit">
 
    <div class="mb-3">
      <label class="form-label">Username</label>
      <input type="text" v-model="request.userName" class="form-control" placeholder="Enter username">
    </div>

     <div class="mb-3">
      <label class="form-label">Email Address</label>
      <input type="email" v-model="request.emailAddress" class="form-control" placeholder="Enter email address">
    </div>

     <div class="mb-3">
      <label class="form-label">Password</label>
      <input type="password" v-model="request.password" class="form-control" placeholder="Enter password">
    </div>

     <div class="mb-3">
      <label class="form-label">Confirm Password</label>
      <input type="password" v-model="request.confirmPassword" class="form-control" placeholder="Enter password">
    </div>

    {{request.userName}}
{{request.emailAddress}}
    <button type="submit" class="btn btn-primary">Submit</button>
 
</form>
</template>

<script>
import { reactive, ref } from 'vue';
import useAuth from '@/composables/FoodRecord/auth.js';


export default {
    components: {
        
    },
    setup() {

     
        const {register} = useAuth();

        const request = reactive({
            userName: "",
            emailAddress: "",
            password: "",
            confirmPassword: ""
        })

        const handleSubmit = () => {
            register(request);
           
        }

        return {
            request, handleSubmit
        }
    }

}
</script>

<style>

</style>