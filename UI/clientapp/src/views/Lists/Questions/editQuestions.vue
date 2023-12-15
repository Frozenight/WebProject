<template>
    <div class="edit-questions-container">
      <h1>Edit Questions for Quiz ID: {{ quizId }}</h1>
      <button class="btn btn-primary" @click="createQuestion">Create New Question</button>
  
      <div v-if="questions.length">
        <div class="quizzes-list">
          <div class="col" v-for="question in questions" :key="question.id">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">{{ question.text }}</h5>
                <!-- Add additional question details if any -->
                <button class="btn btn-primary" @click="editQuestion(question.id)">Edit</button>
                <button class="btn btn-danger" @click="deleteQuestion(question.id)">Delete</button>
                <button class="btn btn-secondary" @click="editAnswers(question.id)">Edit Answers</button>
              </div>
            </div>
          </div>
          <button class="btn btn-secondary" type="button" @click="goBack">Back</button>
        </div>
      </div>
  
      <div v-else>
        <p>No questions available for this quiz.</p>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import { useRouter, useRoute } from 'vue-router';
  
  const router = useRouter();
  const route = useRoute();
  const quizId = route.params.id;
  const questions = ref([]);
  const localurl = "https://localhost:44340";
  
  onMounted(async () => {
    try {
      const response = await axios.get(`https://localhost:44340/api/quizes/${quizId}/questions`);
      questions.value = response.data;
    } catch (error) {
      console.error('Error fetching questions:', error);
    }
  });

    function createQuestion() {
        router.push({ name: 'createQuestion', params: { id: quizId } });
    }

    // Edit a question
    function editQuestion(questionId) {
        router.push({ name: 'editQuestion', params: { id: questionId} });
    }

    function editAnswers(questionId) {
        router.push({ name: 'editAnswers', params: { quizId: quizId, questionId: questionId} });
    }

    function goBack() {
        router.back();
    }


    // Delete a question
    async function deleteQuestion(questionId) {
        try {
            const token = localStorage.getItem('token');
            await axios.delete(`${localurl}/api/quizes/question/${questionId}`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
            });
            // Optionally refresh the list of questions without reloading the page
            questions.value = questions.value.filter(q => q.id !== questionId);
        } catch (error) {
            console.error('Error deleting question:', error);
        }
    }
  </script>
  
  <style scoped>
  /* Add your CSS here */
  </style>
  