#!/bin/bash

PROJECT="./Presentation/Netflex.WebAPI/Netflex.WebAPI.csproj"

[ ! -f ".env" ] && echo "❌ .env not found" && exit 1
while IFS='=' read -r key value || [[ -n $key ]]; do
  [[ "$key" =~ ^#.*$ || -z "$key" || -z "$value" ]] && continue

  key=$(echo "$key" | xargs | sed 's/__/:/g')
  value=$(echo "$value" | sed 's/^"//;s/"$//' | xargs)

  if [[ -n "$key" && -n "$value" ]]; then
    dotnet user-secrets set "$key" "$value" --project "$PROJECT"
  fi
done < .env

echo "✅ Done converting .env to user-secrets"