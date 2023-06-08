<template>
  <div class="search-bar-container">
    <input type="text" placeholder="Search for a Food Record.." @change="sendChange" v-model="query"/>
  </div>
</template>

<script>
import { ref, watch } from 'vue';

export default {
    props: [],
    emits: ["onHandleChange"],
    setup(props, {emit}) {
        const query = ref(null);

      
        const sendChange = () => {
            emit('onHandleChange', query.value);
        }
        watch(query, sendChange)
        return {
            query, sendChange
        }
    }
}
</script>

<style scoped>

    input::placeholder {
        font-size: .85rem;
    }

    input {
        max-width: 300px;
        border-radius: 10px;
        background-color: rgb(238, 238, 238);
        border: 2px solid rgb(207, 207, 207);
        box-shadow: none;

        padding: .75rem;
    }
</style>