<template>
<nav class="navbar navbar-expand-lg navbar-light bg-light">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">Food Tracker</a>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarNav">
      <ul class="navbar-nav">
        <li class="nav-item">
          <a class="nav-link active" aria-current="page" href="#" @click.prevent="login" v-if="!user">Login</a>
        </li>
        <li class="nav-item">
            <a class="nav-link active" aria-current="page" href="#" @click.prevent="logout">Logout</a>
        </li>

      </ul>
    </div>
  </div>
</nav>
</template>

<script>
import { ref } from 'vue';


    export default {
        components: {

        },
        setup() {
            let user = ref(null)


            const login = () => {

            }

            const logout = () => {
                
            }
        }
    }

</script>

<style lang="scss" scoped>

</style>