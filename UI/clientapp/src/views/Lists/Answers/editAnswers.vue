<template>
    <div class="edit-answers-container">
      <h1>Edit Answers for Question ID: {{ questionId }}</h1>
      <button @click="createAnswer">Create New Answer</button>
  
      <div v-if="answers.length">
        <div class="answers-list">
          <div class="col" v-for="answer in answers" :key="answer.id">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">{{ answer.text }}</h5>
                <p class="card-text">Correct: {{ answer.isCorrect ? 'Yes' : 'No' }}</p>
                <button class="btn btn-primary" @click="editAnswer(answer.id)">Edit</button>
                <button class="btn btn-danger" @click="deleteAnswer(answer.id)">Delete</button>
              </div>
            </div>
          </div>
          <button type="button" @click="goBack">Back</button>
        </div>
      </div>
  
      <div v-else>
        <p>No answers available for this question.</p>
        <button type="button" @click="goBack">Back</button>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import { useRouter, useRoute } from 'vue-router';
  
  const router = useRouter();
  const route = useRoute();
  const questionId = route.params.questionId;
  const quizId = route.params.quizId; // If needed
  const answers = ref([]);
  const localurl = "https://localhost:44340";
  
  onMounted(async () => {
    try {
      const response = await axios.get(`https://localhost:44340/api/quizes/${quizId}/questions/${questionId}/answers`);
      answers.value = response.data;
    } catch (error) {
      console.error('Error fetching answers:', error);
    }
  });

  function createAnswer() {
        router.push({ name: 'createAnswer', params: { quizId: quizId, id: questionId } });
    }

    function editAnswer(answerId) {
        router.push({ name: 'editAnswer', params: { quizId: quizId, id: questionId, answerId: answerId} });
    }

    async function deleteAnswer(answerId) {
        try {
            const token = localStorage.getItem('token');
            await axios.delete(`${localurl}/api/quizes/answer/${answerId}`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
            });
            // Optionally refresh the list of questions without reloading the page
            answers.value = answers.value.filter(q => q.id !== answerId);
        } catch (error) {
            console.error('Error deleting question:', error);
        }
    }

    function goBack() {
        router.back();
    }
  </script>
  
  <style scoped>
  /* Add your CSS here */
  </style>
  