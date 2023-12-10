<template>
    <div class="edit-quiz-container">
      <h1>Edit Quiz</h1>
      <form @submit.prevent="submitQuiz">
        <div>
          <label for="name">Quiz Name:</label>
          <input type="text" id="name" v-model="quiz.name" required>
        </div>
        <div>
          <label for="description">Description:</label>
          <textarea id="description" v-model="quiz.description" required></textarea>
        </div>
        <div>
          <label for="category">Category:</label>
          <input type="text" id="category" v-model="quiz.category" required>
        </div>
        <button @click="submitQuiz">Update Quiz</button>
        <button @click="goBack">Back</button>
      </form>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import { useRouter, useRoute } from 'vue-router';
  
  const router = useRouter();
  const route = useRoute();
  const quiz = ref({
    name: '',
    description: '',
    category: ''
  });
  const localurl = "https://localhost:44340";
  const quizId = route.params.id; // assuming the quiz ID is passed as a route parameter
  
  // Fetch quiz data
  onMounted(async () => {
        const token = localStorage.getItem('token');

        if (!token) {
            console.error('No token found');
            return;
        }

        const config = {
            headers: { Authorization: `Bearer ${token}` }
        };

        try {
            const response = await axios.get(`${localurl}/api/quizes/${quizId}`, config);
            quiz.value = response.data.resource; // Modify this line
        } catch (error) {
            console.error('Error fetching quiz:', error);
        }
    });

  
  async function submitQuiz() {
    try {
      const token = localStorage.getItem('token');
      const config = {
        headers: { Authorization: `Bearer ${token}` }
      };
  
      await axios.put(`${localurl}/api/quizes/${quizId}`, quiz.value, config);
      // Handle successful update (e.g., showing a success message or redirecting)
      router.push({ name: 'admin' });
    } catch (error) {
      console.error('Error updating quiz:', error);
      // Handle errors (e.g., showing an error message)
    }
  }

  function goBack() {
    router.push({ name: 'admin'});
  }

  function editQuestions(quizId) {
    router.push({ name: 'editQuestions', params: { id: quizId } });
  }
  </script>
  
  <style scoped>
  /* Add your CSS here */
  </style>
  