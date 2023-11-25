import { createRouter, createWebHistory } from 'vue-router'
import QuizesList from '../views/Lists/quizesList.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: QuizesList
    }
  ]
})

export default router
