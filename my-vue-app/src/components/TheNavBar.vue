<script setup lang="ts">
import { useWindowScroll } from "@vueuse/core";

var { y } = useWindowScroll();
</script>

<template>
	<div class="wrapper">
		<v-bottom-navigation grow class="bottom-nav" :elevation="24" rounded="xl">
			<v-spacer></v-spacer>
			<v-btn @click="y = 0" to="/">
				<v-icon>mdi-home</v-icon>
				<span>Home</span>
			</v-btn>

			<v-btn to="/connect">
				<v-icon>mdi-account-search</v-icon>
				<span>Connect</span>
			</v-btn>

			<v-btn to="/societies">
				<v-icon>mdi-basketball</v-icon>
				<span>Societies</span>
			</v-btn>

			<v-btn to="/courses">
				<v-icon>mdi-account-multiple</v-icon>
				<span>Courses</span>
			</v-btn>

			<v-spacer></v-spacer>
		</v-bottom-navigation>
	</div>
</template>

<style scoped>
.wrapper {
	position: relative;
	display: flex;
	justify-content: center;
}

.bottom-nav {
	margin-bottom: 10px;
	width: 1000px !important;
	left: auto !important;
	z-index: 1000;
}
</style>
