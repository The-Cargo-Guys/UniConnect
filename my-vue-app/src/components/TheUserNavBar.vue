<script setup lang="ts">
import { defineEmits } from "vue";

const emit = defineEmits(["logout"]);
</script>
<template>
	<div class="wrapper">
		<v-toolbar floating class="nav" flat mode="shift" rounded="xl">
			<v-spacer></v-spacer>
			<v-btn to="/profile" flat>
				<v-icon>mdi-account</v-icon>
				<span>Profile</span>
			</v-btn>
			<v-btn @click="$emit('logout')" flat>Log Out</v-btn>
			<v-spacer></v-spacer>
		</v-toolbar>
	</div>
</template>

<style scoped>
.wrapper {
	position: relative;
	display: flex;
	justify-content: center;
}

.nav {
  background-color: white;
	position: fixed;
	margin-bottom: 10px;
	max-width: 250px;
	right: 0.5% !important;
	z-index: 1000;
	top: 10px !important;
}
</style>
