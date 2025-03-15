<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';
const { isAuthenticated } = useAuth0();
</script>


<template>
    <v-card v-if="isAuthenticated"><h1>YOU ARE STILL LOGGED IN</h1></v-card>
    <div>
        <p>Communities</p>

    </div>
</template>