<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';
import { HelloWorld } from '../components';

const { loginWithRedirect, isAuthenticated, logout, user } = useAuth0();

</script>

<template>
    <v-card>
        <v-row v-if="!isAuthenticated">
            <v-btn class="ma-5" label="Login" text="Login" @click="loginWithRedirect"></v-btn>
            <v-btn class="ma-5" label="Sign Up" text="Sign Up" @click="loginWithRedirect({authorizationParams: { screen_hint: 'signup' }})"></v-btn>
        </v-row>
        <v-row v-else>
            <v-card-title>LOGGED IN</v-card-title>
            <v-card-title>Welcome {{ user?.email }}</v-card-title>
            <v-btn class="ma-5" label="Log Out" text="Log Out" @click="logout"></v-btn>
        </v-row>
    </v-card>
    <p>Welcome Home!</p>
    <HelloWorld msg="Users"></HelloWorld>
</template>
