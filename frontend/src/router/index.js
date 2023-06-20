import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import UpdateFood from '../components/UpdateFood.vue'
import AddFood from '../components/AddFood.vue'
import SignUpForm from '../components/SignUpForm.vue'
import LoginForm from '../components/LoginForm.vue'
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
  },
  {
    path: '/sign-up',
    name: 'signup',
    component: SignUpForm
  },
  {
    path: '/log-in',
    name: 'login',
    component: LoginForm
  }
  
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
