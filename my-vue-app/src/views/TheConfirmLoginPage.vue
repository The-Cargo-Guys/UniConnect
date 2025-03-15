<script lang="ts">
import axios from 'axios';
import { useRouter } from 'vue-router';
const router = useRouter();

export default {
    data() {
        return {
            verificationCode: '',
        }
    },
    methods: {
        CheckCode() {
            if (this.verificationCode.length === 6) {
                axios.post("VerificationRequest", this.verificationCode).then(
                    () => {
                        router.push("/");
                    },
                    (error) => {
                        console.log(error.data);
                    }
                )
            }
        }
    },
    watch: {
        verificationCode: "CheckCode"
    }
}
</script>

<template>
    <v-card>
        <v-card-title>WELCOME TO UNICENTRE</v-card-title>
    </v-card>
    <v-container :style="{ width: '600px'}">
        <v-card color="primary" class="pl-10 pr-10 pb-5">
                <v-card-title>CONFIRM LOGIN USING 2FA</v-card-title>
                <p class="mb-5">To confirm your login, 6-digit code (XXXXXX) has been sent to your email, please enter this below before the timer runs out.</p>
                <p>Not seeing the code?  Make sure to check your spam folder, else you can request a new code be sent <v-btn @click="sendNewRequest">Here</v-btn></p>

                <v-text-field variant="solo" label="Please Enter the 6-Digit Code" class="ma-5" v-model="verificationCode"></v-text-field>
            </v-card>
    </v-container>
</template> 