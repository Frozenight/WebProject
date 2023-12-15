<template>
    <div class="create-question-container">
      <h1>Create New Question for Quiz ID: {{ quizId }}</h1>
      <form @submit.prevent="submitQuestion">
        <div>
          <label for="questionText">Question:</label>
          <input type="text" id="questionText" v-model="question.text" required>
        </div>
        <!-- Add more fields if necessary -->
        <button class="btn btn-primary"  type="submit">Create Question</button>
        <button  class="btn btn-secondary"   type="button" @click="goBack">Back</button>
      </form>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import axios from 'axios';
  import { useRouter, useRoute } from 'vue-router';
  
  const router = useRouter();
  const route = useRoute();
  const quizId = route.params.id;
  const question = ref({
    text: '' // Add more fields as necessary
  });
  const localurl = "https://localhost:44340";
  
  async function submitQuestion() {
    try {
        const token = localStorage.getItem('token');
        const config = {
        headers: { Authorization: `Bearer ${token}` }
        };

        // Update the data structure to match your API requirements
        const questionData = {
        Text: question.value.text, // Assuming question.value.text holds the question text
        QuizId: quizId
        };

        // Update the endpoint URL
        await axios.post(`${localurl}/api/quizes/question`, questionData, config);

        // Handle successful creation
        router.push({ name: 'editQuestions', params: { id: quizId } });
    } catch (error) {
        console.error('Error creating question:', error);
        // Handle errors
    }
    }

    function goBack() {
    router.back();
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
  