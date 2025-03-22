<script setup lang="ts">
import { useRouter, useRoute } from "vue-router";
import { RouterView } from "vue-router";
import { TheNavBar, TheTopNavBar } from "./components";
import Auth from "./views/Auth.vue";
import { ref, onMounted, watch } from "vue";

const router = useRouter();
const route = useRoute();
const isLoggedIn = ref(false);

// Auth before rendering
onMounted(() => {
	const userId = localStorage.getItem("userId");

	if (userId) {
		console.log("✅ Found stored userId, setting isLoggedIn = true");
		isLoggedIn.value = true;
	} else {
		console.log("❌ No userId found, checking if on /auth");
		isLoggedIn.value = false;

		if (route.path !== "/auth") {
			router.push("/auth");
		}
	}
});

// Watch localStorage for login/logout changes
watch(
	() => localStorage.getItem("userId"),
	(newUserId) => {
		if (newUserId) {
			console.log("🔄 userId updated, setting isLoggedIn = true");
			isLoggedIn.value = true;
			router.push("/");
		} else {
			console.log("❌ userId removed, redirecting to /auth");
			isLoggedIn.value = false;
			router.push("/auth");
		}
	}
);

const handleLogout = () => {
	localStorage.removeItem("userId");
	sessionStorage.clear();
	isLoggedIn.value = false;
	router.push("/auth");
};
</script>

<template>
	<v-app class="fade-in" ref="el">
		<template v-if="!isLoggedIn">
			<Auth />
		</template>
		<template v-else>
			<TheNavBar @logout="handleLogout"/>
			<TheTopNavBar @logout="handleLogout" />
			<v-main>
				<RouterView />
			</v-main>
		</template>
	</v-app>
</template>

<style scoped>
.loading-screen {
	display: flex;
	justify-content: center;
	align-items: center;
	height: 100vh;
	font-size: 20px;
	font-weight: bold;
}

.logo-wrapper {
	position: fixed;
	background-color: rgb(255, 255, 255);
	margin-top: 10px;
	height: 110px;
	width: 110px;
}
</style>
