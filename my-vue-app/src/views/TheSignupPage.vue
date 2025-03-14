<script lang="ts">
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();

export default {
    data() {
        return {
            isSubmissionFaliure: false,
            submissionError: '',

            carousel: 0,
            
            forename: '',
            surname: '',
            username: '',
            birthYear: new Date().getFullYear(),
            birthMonth: new Date().getMonth(),
            birthDay: 1,
            gender: '',
            major: '',
            email: '',
            password: '',
            confirmPassword: '',

            days: [],
            months: Array.from({length: 12}, (_, i) => ((i + 1))),
            years: Array.from({length: 106}, (_, i) => ((1920 + i)))
        }
    },
    methods: {
        getDate() {

        },
        
        isFormValid() {
            if (this.password === this.confirmPassword) {
                console.log("123");
                return true;
            }

            return false;
        },

        submitSignUp() {
            if (!this.isFormValid()) {
                this.isSubmissionFaliure = true,
                this.submissionError = "Password and Confirm Password do not match."
                console.log("123");
                return;
            }

            const signUpForm = {
                forename: this.forename,
                surname: this.surname,
                username: this.username,
                birthYear: this.birthYear,
                birthMonth: this.birthMonth,
                birthDay: this.birthDay,
                gender: this.gender,
                major: this.major,
                email: this.email,
                password: this.password,
            }

            axios.post("signupURL", signUpForm).then(
                () => { router.push("/"); },
                (error) => {
                    this.isSubmissionFaliure = true;
                    this.submissionError = error.data;
                }
            );
        },

        updateDaysInMonth() {
            const d = new Date(this.birthYear, this.birthMonth + 1, 0);
            this.days = Array.from({length: d.getDate()}, (_, i) => ((i + 1)));
        }
    },
    watch: {
        birthYear: 'updateDaysInMonth',
        birthMonth: 'updateDaysInMonth'
    }
}
</script>

<template>
    <v-card>
        <v-card-title>WELCOME TO UNICENTRE</v-card-title>
    </v-card>
    <v-container :style="{ width: '600px'}">
        <v-card color="primary" class="pl-10 pr-10 pb-5">
            <v-card-title>SIGN-UP</v-card-title>
            <v-form @submit.prevent="submitSignUp">
                <v-carousel
                    v-model="carousel"
                    progress="secondary"
                    :continuous="false"
                    :show-arrows="false"
                    delimiter-icon="mdi-square"
                    hide-delimiter-background
                    hide-delimiters
                >
                    <v-carousel-item>
                            <v-text-field label="Forename" variant="underlined" solo flat v-model="forename" required></v-text-field>
                            <v-text-field label="Surname" variant="underlined" v-model="surname" required></v-text-field>
                            <v-text-field label="Username" variant="underlined" v-model="username" required></v-text-field>
                            <v-card-subtitle>Date Of Birth</v-card-subtitle>
                            <v-row>
                                <v-col cols="12" sm="4">
                                    <v-select 
                                        label="Year"
                                        v-model="birthYear"
                                        @change="updateDaysInMonth"
                                        :items="years">
                                    </v-select>
                                </v-col>
                                <v-col cols="12" sm="4">
                                    <v-select 
                                        label="Month"
                                        v-model="birthMonth"
                                        @change="updateDaysInMonth"
                                        :items="months">
                                    </v-select>
                                </v-col>
                                <v-col cols="12" sm="4">
                                    <v-select 
                                        label="Day"
                                        :items="days">
                                    </v-select>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12" sm="4">
                                    <v-select 
                                    label="Gender"
                                    v-model="gender"
                                    :items="['Male', 'Female']" required>
                                </v-select>
                                </v-col>
                                <v-col cols="12" sm="8">
                                    <v-text-field label="Major" variant="underlined" v-model="major" required></v-text-field>
                                </v-col>
                            </v-row>
                            <v-btn label="next" text="next" color="secondary" @click="carousel = 1" class="ma-10"></v-btn>
                    </v-carousel-item>
                    <v-carousel-item>
                        <v-text-field label="Email" v-model="email" type="email" variant="underlined" required></v-text-field>
                        <v-text-field label="Password" v-model="password" type="password" variant="underlined" required></v-text-field>
                        <v-text-field label="Confirm Password" v-model="confirmPassword" variant="underlined" type="password" required></v-text-field>    
                        <v-card-subtitle v-if="isSubmissionFaliure" :style="{ color: 'red'}">{{ submissionError }}</v-card-subtitle> 
                        <v-btn color="secondary" type="submit" @click="submitSignUp" block>Sign-Up</v-btn>
                        <v-btn label="back" text="back" color="secondary" @click="carousel = 0" class="ma-10"></v-btn>
                    </v-carousel-item>
                </v-carousel>
            </v-form>
        </v-card>
    </v-container>
</template> 