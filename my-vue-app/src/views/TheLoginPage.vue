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
    <v-container>
        <v-form @submit.prevent="submitLogin">
            <v-card>
                <v-card-title>WECLOME</v-card-title>
                <v-form>
                    <v-text-field label="Email" type="email" required></v-text-field>
                    <v-text-field label="Password" type="password" required></v-text-field>

                    <v-btn color="primary" type="submit" @click="submitLogin" block>Login</v-btn>
                </v-form>

                <v-card-subtitle>If you don't have an account, you can <v-Btn color="secondary" href="/signup">sign-up</v-Btn> here.</v-card-subtitle>
            </v-card>
        </v-form>
    </v-container>
</template> 