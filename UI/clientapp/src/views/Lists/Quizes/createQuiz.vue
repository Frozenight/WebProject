<template>
    <div class="create-quiz-container">
      <h1>Create New Quiz</h1>
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
        <button type="submit">Create Quiz</button>
      </form>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import axios from 'axios';
  import { useRouter } from 'vue-router';

  const router = useRouter();
  
  const quiz = ref({
    name: '',
    description: '',
    category: ''
  });
  
  const localurl = "https://localhost:44340";
  
  async function submitQuiz() {
    try {
      const token = localStorage.getItem('token'); // Get the JWT token from localStorage
      const config = {
        headers: { Authorization: `Bearer ${token}` }
      };
      
      await axios.post(localurl + "/api/quizes/", quiz.value, config); // Include config in the request
      // Handle successful creation (e.g., showing a success message or redirecting)
      router.push({ name: 'admin' });
    } catch (error) {
      console.error('Error creating quiz:', error);
      // Handle errors (e.g., showing an error message)
    }
  }

  </script>
  
  <style scoped>
  /* Add your CSS here */
  </style>
  