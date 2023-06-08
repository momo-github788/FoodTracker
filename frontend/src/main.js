import { createApp } from "vue";
import App from "./App.vue";
import vSelect from "vue-select";
import Pagination from 'v-pagination-3';
import "vue-select/dist/vue-select.css";
import router from './router'
import 'vue-awesome/icons'

const app = createApp(App);

app.component("v-select", vSelect)
app.component('pagination', Pagination);
app.use(router)
app.mount('#app')
