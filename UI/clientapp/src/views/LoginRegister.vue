<template>
    <div>
      <!-- Login Form -->
      <form @submit.prevent="login">
        <h2>Login</h2>
        <input type="text" v-model="loginUsername" placeholder="Username" />
        <input type="password" v-model="loginPassword" placeholder="Password" />
        <button type="submit">Login</button>
        <p v-if="loginFeedback" :class="{'success-message': loginSuccess, 'error-message': !loginSuccess}">
          {{ loginFeedback }}
        </p>
      </form>
  
      <!-- Registration Form -->
      <form @submit.prevent="register">
        <h2>Register</h2>
        <input type="text" v-model="registerUsername" placeholder="Username" />
        <input type="email" v-model="registerEmail" placeholder="Email" />
        <input type="password" v-model="registerPassword" placeholder="Password" />
        <button type="submit">Register</button>
        <p v-if="registerFeedback">{{ registerFeedback }}</p>
      </form>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    data() {
      return {
        // Data properties for login
        loginUsername: '',
        loginPassword: '',
        loginFeedback: '',
        loginSuccess: false,
  
        // Data properties for registration
        registerUsername: '',
        registerEmail: '',
        registerPassword: '',
        registerFeedback: '',
      };
    },
    methods: {
      async login() {
        try {
          const response = await axios.post('https://localhost:44340/api/login', {
            userName: this.loginUsername,
            password: this.loginPassword
          });
          this.loginFeedback = 'Login successful!';
          this.loginSuccess = true;
          localStorage.setItem('username', this.loginUsername); // Adjust according to your API response
          localStorage.setItem('token', response.data.accessToken);

          this.$router.push({ name: 'home' });
          // Additional logic for successful login (e.g., storing token, redirect)
        } catch (error) {
          this.loginFeedback = 'Login failed. Please check your credentials.';
          this.loginSuccess = false;
        }
      },
      async register() {
        try {
          const response = await axios.post('https://localhost:44340/api/register', {
            userName: this.registerUsername,
            email: this.registerEmail,
            password: this.registerPassword
          });
          this.registerFeedback = 'Registration successful! Please log in.';
        } catch (error) {
          this.registerFeedback = 'Registration failed. Please try again.';
        }
      }
    }
  };
  </script>
  
  <style scoped>
  .success-message {
    color: green;
  }
  
  .error-message {
    color: red;
  }
  </style>
  