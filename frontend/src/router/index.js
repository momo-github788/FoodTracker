import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import UpdateFood from '../components/UpdateFood.vue'
import AddFood from '../components/AddFood.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/update/:id',
    name: 'update',
    component: UpdateFood,
    props: true
  },
  {
    path: '/add',
    name: 'add',
    component: AddFood
  }
  
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
