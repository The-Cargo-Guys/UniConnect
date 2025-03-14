<script lang="ts">
import axios from 'axios';

export default {
    data() {
        return {
            forename: '',
            surname: '',
            username: '',
            birthYear: 2000,
            birthMonth: 1,
            birthDay: 1,
            major: '',
            email: '',
            password: '',
            confirmPassword: '',

            days: Array.from({length: 31}, (_, i) => ((i + 1))),

            months: Array.from({length: 12}, (_, i) => ((i + 1))),

            years: Array.from({length: 106}, (_, i) => ((1920 + i)))
        }
    },
    methods: {
        isFormValid() {
            if (this.password !== this.currentPassword) {
                return false;
            }

            return true;
        },

        submitSignUp() {
            if (this.isFormValid()) {
                return;
            }

            const signUpForm = {
                forename: this.forename,
                surname: this.surname,
                username: this.username,
                birthYear: this.birthYear,
                birthMonth: this.birthMonth,
                birthDay: this.birthDay,
                email: this.email,
                password: this.password,
            }
            console.log("gi");
            axios.post("signupURL", signUpForm).then(
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
        <v-card class="pl-10 pr-10 pb-5">
            <v-card-title>SIGN-UP</v-card-title>
            <v-form @submit.prevent="submitSignUp">
                <v-carousel
                    :continuous="false"
                    :show-arrows="false"
                >
                    <v-carousel-item>
                            <v-text-field label="Forename" v-model="forename" required></v-text-field>
                            <v-text-field label="Surname" v-model="surname" required></v-text-field>
                            <v-text-field label="Username" v-model="username" required></v-text-field>
                            <v-text>Date Of Birth</v-text>
                            <v-row>
                                <v-col cols="12" sm="4">
                                    <v-select 
                                        label="Year"
                                        v-model="birthYear"
                                        :items="years">
                                    </v-select>
                                </v-col>
                                <v-col cols="12" sm="4">
                                    <v-select 
                                        label="Month"
                                        v-model="birthMonth"
                                        :items="months">
                                    </v-select>
                                </v-col>
                                <v-col cols="12" sm="4">
                                    <v-select 
                                        label="Day"
                                        v-model="birthDay"
                                        :items="days">
                                    </v-select>
                                </v-col>
                            </v-row>
                            <v-text-field label="Major" v-model="major" required></v-text-field>
                    </v-carousel-item>
                    <v-carousel-item>
                        <v-text-field label="Email" v-model="email" type="email" required></v-text-field>
                        <v-text-field label="Password" v-model="password" type="password" required></v-text-field>
                        <v-text-field label="Confirm Password" v-model="confirmPassword" type="password" required></v-text-field>     
                        <v-btn color="primary" type="submit" @click="submitSignUp" block>Sign-Up</v-btn>
                    </v-carousel-item>
                </v-carousel>
            </v-form>
        </v-card>
    </v-container>
</template> 