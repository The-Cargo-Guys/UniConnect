<script setup lang="ts">
import { useAuth0 } from "@auth0/auth0-vue";
import { useRouter } from "vue-router";
import { RouterView } from "vue-router";
import TheNavBar from "./components/TheNavBar.vue";
import AuthPage from "./views/AuthPage.vue";
import { computed, watchEffect, ref, onMounted, watch } from "vue";
import TheUserNavBar from "./components/TheUserNavBar.vue";

const { loginWithRedirect, logout, isAuthenticated, isLoading } = useAuth0();
const router = useRouter();
const isLoggedIn = ref(false);

// 🚀 **Check authentication on app load BEFORE rendering**
onMounted(() => {
	const token = localStorage.getItem("token");

	if (token) {
		console.log("✅ Found stored token, setting isLoggedIn = true");
		isLoggedIn.value = true;
	} else {
		console.log("❌ No token found, redirecting to /auth");
		isLoggedIn.value = false;
		router.push("/auth");
	}
});

// 🔄 **Watch localStorage for instant updates**
watch(
	() => localStorage.getItem("token"),
	(newToken) => {
		if (newToken) {
			console.log("🔄 Token updated, setting isLoggedIn = true");
			isLoggedIn.value = true;
			router.push("/");
		} else {
			console.log("❌ Token removed, redirecting to /auth");
			isLoggedIn.value = false;
			router.push("/auth");
		}
	}
);

// 🔄 **Watch Auth0 authentication changes dynamically**
watchEffect(() => {
	console.log("🔄 Auth state changed:", isAuthenticated.value);

	if (isAuthenticated.value) {
		localStorage.setItem("token", "auth0-user"); // Simulate storing token
		isLoggedIn.value = true;
		router.push("/");
	} else if (!localStorage.getItem("token")) {
		localStorage.removeItem("token");
		isLoggedIn.value = false;
		router.push("/auth");
	}
});

// ✅ **Handle Logout - Clears storage & cookies**
const handleLogout = () => {
	// Clear all local storage, session storage, and cookies
	localStorage.clear();
	sessionStorage.clear();

	document.cookie.split(";").forEach((c) => {
		document.cookie = c
			.replace(/^ +/, "")
			.replace(/=.*/, `=;expires=${new Date(0).toUTCString()};path=/`);
	});

	isLoggedIn.value = false; // Update UI state
};
</script>

<template>
	<v-app class="fade-in">
		<!-- ⏳ Show loading state -->
		<template v-if="isLoading">
			<div class="loading-screen">Loading...</div>
		</template>

		<!-- 🔑 Show login/register page if user is NOT authenticated -->
		<template v-else-if="!isLoggedIn">
			<AuthPage />
		</template>

		<!-- 🏠 Show main content when the user IS authenticated -->
		<template v-else>
			<TheNavBar />
			<TheUserNavBar @logout="handleLogout"/>
			<v-avatar class="logo-wrapper">
				<v-img
					src="/UniConnect.svg"
					alt="Logo"
					contain
				></v-img>
			</v-avatar>
			<v-spacer></v-spacer>
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
	position:fixed;
	height: 100px;
	width: 100px;
	left: 0;
	top: 0;
}
</style>
