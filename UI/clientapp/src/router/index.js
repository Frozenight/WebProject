import { createRouter, createWebHistory } from 'vue-router'
import QuizesList from '../views/Lists/quizesList.vue'
import LoginRegister from '../views/LoginRegister.vue'
import Admin from '../views/Lists/admin.vue'
import CreateQuiz from '../views/Lists/Quizes/createQuiz.vue'
import EditQuiz from '../views/Lists/Quizes/editQuiz.vue'
import EditQuestions from '../views/Lists/Questions/editQuestions.vue'
import EditQuestion from '../views/Lists/Questions/editQuestion.vue'
import CreateQuestion from '../views/Lists/Questions/createQuestions.vue'
import EditAnswers from '../views/Lists/Answers/editAnswers.vue'
import CreateAnswer from '../views/Lists/Answers/CreateAnswer.vue'
import EditAnswer from '../views/Lists/Answers/editAnswer.vue'
import Quiz from '../views/Lists/quiz.vue'

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
    },
    {
      path: '/edit-questions/:id',
      name: 'editQuestions',
      component: EditQuestions
    },
    {
      path: '/edit-question/:id/',
      name: 'editQuestion',
      component: EditQuestion
    },
    {
      path: '/create-question/:id',
      name: 'createQuestion',
      component: CreateQuestion
    },
    {
      path: '/create-answers/:quizId/:id',
      name: 'createAnswer',
      component: CreateAnswer
    },
    {
      path: '/edit-answers/:quizId/:questionId',
      name: 'editAnswers',
      component: EditAnswers
    },
    {
      path: '/edit-answer/:quizId/:questionId/:answerId',
      name: 'editAnswer',
      component: EditAnswer
    },
    {
      path: '/quiz/:id',
      name: 'quiz',
      component: Quiz
    }
    
  ]
})

export default router
