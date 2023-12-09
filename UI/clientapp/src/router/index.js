import { createRouter, createWebHistory } from 'vue-router'
import QuizesList from '../views/Lists/quizesList.vue'
import LoginRegister from '../views/LoginRegister.vue'
import Admin from '../views/Lists/admin.vue'
import CreateQuiz from '../views/Lists/Quizes/createQuiz.vue'
import EditQuiz from '../views/Lists/Quizes/editQuiz.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: QuizesList
    },
    {
      path: '/login', // Define the path for the login/register page
      name: 'login',
      component: LoginRegister // Assign the LoginRegister component to this route
    },
    {
      path: '/admin',
      name: 'admin',
      component: Admin
    },
    {
      path: '/createQuiz',
      name: 'createQuiz',
      component: CreateQuiz
    },
    {
      path: '/editQuiz/:id',
      name: 'editQuiz',
      component: EditQuiz
    }
    // You can add more routes here
  ]
})

export default router
