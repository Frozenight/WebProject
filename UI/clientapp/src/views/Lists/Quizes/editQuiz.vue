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
          <input type="text" id="description" v-model="quiz.description" required>
        </div>
        <div>
          <label for="category">Category:</label>
          <input type="text" id="category" v-model="quiz.category" required>
        </div>
        <button class="btn btn-primary" @click="submitQuiz">Update Quiz</button>
        <button class="btn btn-secondary" @click="goBack">Back</button>
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

  </script>
  
  <style scoped>
       .container {
            font-family: Arial, sans-serif;
            margin: 20px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }

        input[type="text"] {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        input[type="checkbox"] {
            margin-right: 10px;
        }
  </style>
  