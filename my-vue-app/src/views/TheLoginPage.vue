<script lang="ts">
import axios from 'axios';
import { useRouter } from 'vue-router';
const router = useRouter();

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
            console.log(this.email);
            return this.email.includes('@');
        },
        
        submitLogin() {
            if (!this.isFormValid()) {
                console.log("123");
                this.isSubmissionFaliure = true;
                this.submissionFalure = 'Invalid login.  Please check the email and password, and try again';
                return;
            }

            const loginForm = {
                email: this.email,
                password: this.password,
            };

            axios.post("loginUrl", loginForm).then(
                () => {
                    router.push("/");
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
        <v-card color="primary" class="pl-10 pr-10 pb-5">
                <v-card-title>LOGIN</v-card-title>
                <v-form @submit.prevent="submitLogin">
                    <v-text-field label="Email" type="email" variant="underlined" v-model="email" required></v-text-field>
                    <v-text-field label="Password" type="password" variant="underlined" v-model="password" required></v-text-field>
                    <v-card-subtitle v-if="isSubmissionFaliure" :style="{ color: 'red', opacity: '100%'}">{{ submissionFalure }}</v-card-subtitle>
                    <v-btn color="secondary" type="submit" @click="submitLogin" block>Login</v-btn>
                </v-form>

                <v-card-subtitle :style="{opacity: '100%'} " class="mt-5">If you don't have an account, you can <v-Btn color="secondary" href="/signup">sign-up</v-Btn> here.</v-card-subtitle>
            </v-card>
    </v-container>
</template> 