<script lang="ts">
import axios from 'axios';

export default {
    data() {
        return {
            email: '',
            password: ''
        }
    },
    methods: {
        isFormValid() {
            console.log("test");
            return this.email.includes('@');
        },
        
        submitLogin() {
            if (this.isFormValid()) {
                console.log("111");
                return;
            }

            const loginForm = {
                email: this.email,
                password: this.password,
            };

            axios.post("loginUrl", loginForm).then(
                (response) => {
                    var result = response.data;
                    console.log(result);
                },
                (error) => {
                    console.log(error);
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
            <v-card class="pl-10 pr-10 pb-5">
                <v-card-title>LOGIN</v-card-title>
                <v-form>
                    <v-text-field label="Email" type="email" required></v-text-field>
                    <v-text-field label="Password" type="password" required></v-text-field>

                    <v-btn color="primary" type="submit" @click="submitLogin" block>Login</v-btn>
                </v-form>

                <v-card-subtitle class="mt-5">If you don't have an account, you can <v-Btn color="secondary" href="/signup">sign-up</v-Btn> here.</v-card-subtitle>
            </v-card>
        </v-form>
    </v-container>
</template> 