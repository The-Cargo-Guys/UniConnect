<script lang="ts">
import axios from 'axios';

export default {
    data() {
        return {
            email: '',
            password: '',
            isSubmissionFaliure: false,
            submissionFalure: ''
        }
    },
    methods: {
        isFormValid() {
            return this.email.includes('@');
        },
        
        submitLogin() {
            if (this.isFormValid()) {
                this.validationError = 'Invalid login.  Please check the email and password, and try again';
                this.isValidationError = true;
                return;
            }

            const loginForm = {
                email: this.email,
                password: this.password,
            };

            axios.post("loginUrl", loginForm).then(
                (response) => {
                    console.log(response.data);
                },
                (error) => {
                    this.isSubmissionFaliure = true;
                    this.submissionFalure = error.data;
                }
            );
        }
    }
}
</script>

<template>
    <v-card>
        <v-card-title>WELCOME TO UNICENTRE</v-card-title>
    </v-card>
    <v-container :style="{ width: '600px'}">
        <v-form @submit.prevent="submitLogin">
            <v-card color="primary" class="pl-10 pr-10 pb-5">
                <v-card-title>LOGIN</v-card-title>
                <v-form>
                    <v-text-field label="Email" type="email" variant="underlined" required></v-text-field>
                    <v-text-field label="Password" type="password" variant="underlined" required></v-text-field>
                    <v-card-subtitle v-if="isSubmissionFaliure" :style="{ color: 'red'}">{{ submissionFalure }}</v-card-subtitle>
                    <v-btn color="secondary" type="submit" @click="submitLogin" block>Login</v-btn>
                </v-form>

                <v-card-subtitle :style="{opacity: '100%'} " class="mt-5">If you don't have an account, you can <v-Btn color="secondary" href="/signup">sign-up</v-Btn> here.</v-card-subtitle>
            </v-card>
        </v-form>
    </v-container>
</template> 