<template>
  <div class="form-container">
      <!-- Login Form -->
      <form @submit.prevent="login" class="form">
          <h2>Login</h2>
          <input type="text" v-model="loginUsername" placeholder="Username" />
          <input type="password" v-model="loginPassword" placeholder="Password" />
          <button type="submit" class="btn btn-primary">Login</button>
          <p v-if="loginFeedback" :class="{'success-message': loginSuccess, 'error-message': !loginSuccess}">
              {{ loginFeedback }}
          </p>
      </form>

      <!-- Registration Form -->
      <form @submit.prevent="register" class="form">
          <h2>Register</h2>
          <input type="text" v-model="registerUsername" placeholder="Username" />
          <input type="email" v-model="registerEmail" placeholder="Email" />
          <input type="password" v-model="registerPassword" placeholder="Password" />
          <button type="submit" class="btn btn-secondary">Register</button>
          <p v-if="registerFeedback">{{ registerFeedback }}</p>
      </form>
  </div>
</template>
  
  <script>
  import axios from 'axios';
  import * as jwt_decode from 'jwt-decode';


  
  export default {
    data() {
      return {
        // Data properties for login
        loginUsername: '',
        loginPassword: '',
        loginFeedback: '',
        loginRole: '',
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

          // Store the token
          const token = response.data.accessToken;
          localStorage.setItem('token', token);

          // Decode the token
          const decodedToken = jwt_decode.jwtDecode(token);
          const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

          this.loginFeedback = 'Login successful!';
          this.loginSuccess = true;
          localStorage.setItem('username', this.loginUsername); // Adjust according to your API response
          localStorage.setItem('role', userRole); // Adjust according to your API response
          localStorage.setItem('token', response.data.accessToken);

          this.$router.push({ name: 'home' });
          // Additional logic for successful login (e.g., storing token, redirect)
        } catch (error) {
          console.log(jwt_decode);
          this.loginFeedback = error;
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
.form-container {
    width: 100%;
    max-width: 400px;
    margin: 0 auto;
    padding: 20px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

.form {
    margin-bottom: 40px;
}

.form h2 {
    text-align: center;
    margin-bottom: 20px;
}

.form input {
    width: 100%;
    padding: 10px;
    margin-bottom: 10px;
    border: 1px solid #ccc;
    border-radius: 4px;
}

.form button {
    width: 100%;
    padding: 10px;
    border: none;
    border-radius: 4px;
    color: white;
    background-color: #007bff;
    cursor: pointer;
}

.btn-secondary {
    background-color: #6c757d;
}

.success-message {
    color: green;
}

.error-message {
    color: red;
}
  </style>