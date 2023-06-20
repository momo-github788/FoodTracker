<template>
  <form @submit.prevent="handleSubmit">

    <div :v-if="hasErrors">
      <span>
        {{errors?.value}}
      </span>
    
    </div>
    
    <div class="mb-3">
      <label class="form-label">Username</label>
      <input type="text" v-model="request.userName" class="form-control" placeholder="Enter username"/>
    </div>

    <div class="mb-3">
      <label class="form-label">Email Address</label>
      <input type="email" v-model="request.emailAddress" class="form-control" placeholder="Enter email address"/>
    </div>

    <div class="mb-3">
      <label class="form-label">Password</label>
      <input type="password" v-model="request.password" class="form-control" placeholder="Enter password"/>
    </div>

    <div class="mb-3">
      <label class="form-label">Confirm Password</label>
      <input type="password" v-model="request.confirmPassword" class="form-control" placeholder="Enter password"/>
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
    const { register, errors} = useAuth();


    const request = ref({
      userName: "",
      emailAddress: "",
      password: "",
      confirmPassword: ""
    });

 
    // const validateForm = () => {
    
    //  return true;
    // };


    const handleSubmit = async () => {

       // Check the UserName field
      // if (!request.value.userName) {
      //   errors.UserName = (errors?.UserName ?? []).concat("Username is required");
      // }

        await register(request.value);
        console.log(errors.value)


        //console.log(Object.values(errors.value.Password))
       console.log(Object.keys(errors.value).length)
    };

    
    const hasError = computed(() => Object.keys(errors.value).length > 0)


    return {
      errors,
      request,
      handleSubmit,
      hasError
      
    };
  },
};
</script>

<style></style>
