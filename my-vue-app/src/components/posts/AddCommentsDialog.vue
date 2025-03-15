<script setup lang="ts">
const props = defineProps({
	enabled: Boolean,
});

const emit = defineEmits(["update:enabled"]);
</script>

<template>
	<v-dialog
		:model-value="props.enabled"
		@update:model-value="emit('update:enabled', $event)"
		max-width="500"
	>
		<template v-slot:default="{ isActive }">
			<v-card>
				<v-card-text> Add your comment here. </v-card-text>

				<v-card-actions>
					<v-spacer></v-spacer>

					<v-btn text="Close" @click="isActive.value = false"></v-btn>
				</v-card-actions>
			</v-card>
		</template>
	</v-dialog>
</template>
