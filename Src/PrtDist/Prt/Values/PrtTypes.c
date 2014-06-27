#include "PrtTypes.h"

PRT_TYPE PrtMkPrimitiveType(_In_ PRT_TYPE_KIND primType)
{
	switch (primType)
	{
		case PRT_KIND_ANY:
		case PRT_KIND_BOOL:
		case PRT_KIND_EVENT:
		case PRT_KIND_MACHINE:
		case PRT_KIND_INT:
		case PRT_KIND_NULL:
		case PRT_KIND_MODEL:
		{
			PRT_TYPE type;
			type.typeKind = primType;
			return type;
		}
		default:
		{
			PrtAssert(PRT_FALSE, "Expected a primitive type.");
			PRT_TYPE type;
			type.typeKind = PRT_TYPE_KIND_CANARY;
			return type;
		}
	}
}

PRT_TYPE PrtMkForgnType(
	_In_ PRT_GUID              typeTag,
	_In_ PRT_FORGN_CLONE       cloner,
	_In_ PRT_FORGN_FREE        freer,
	_In_ PRT_FORGN_GETHASHCODE hasher,
	_In_ PRT_FORGN_ISEQUAL     eqTester)
{
	PRT_TYPE prtType;
	PrtAssert(cloner != NULL, "Bad cloner");
	PrtAssert(freer != NULL, "Bad freer");
	PrtAssert(hasher != NULL, "Bad hasher");
	PrtAssert(eqTester != NULL, "Bad equality tester");

	prtType.type_union.forgn = (PRT_FORGNTYPE *)PrtMalloc(sizeof(PRT_FORGNTYPE));
	prtType.typeKind = PRT_KIND_FORGN;
	prtType.type_union.forgn->typeTag = typeTag;
	prtType.type_union.forgn->cloner = cloner;
	prtType.type_union.forgn->freer = freer;
	prtType.type_union.forgn->hasher = hasher;
	prtType.type_union.forgn->eqTester = eqTester;

	return prtType;
}

PRT_TYPE PrtMkMapType(_In_ PRT_TYPE domType, _In_ PRT_TYPE codType)
{
	PRT_TYPE prtType;
	PrtAssert(domType.typeKind >= 0 && domType.typeKind < PRT_TYPE_KIND_COUNT, "Invalid type expression");
	PrtAssert(codType.typeKind >= 0 && codType.typeKind < PRT_TYPE_KIND_COUNT, "Invalid type expression");
	prtType.type_union.map = (PRT_MAPTYPE *)PrtMalloc(sizeof(PRT_MAPTYPE));
	prtType.typeKind = PRT_KIND_MAP;
	prtType.type_union.map->domType = PrtCloneType(domType);
	prtType.type_union.map->codType = PrtCloneType(codType);
	return prtType;
}

PRT_TYPE PrtMkNmdTupType(_In_ PRT_UINT32 arity)
{
	PRT_TYPE prtType;
	PrtAssert(arity > 0, "Invalid tuple arity");
	prtType.type_union.nmTuple = (PRT_NMDTUPTYPE *)PrtMalloc(sizeof(PRT_NMDTUPTYPE));
	prtType.typeKind = PRT_KIND_NMDTUP;
	prtType.type_union.nmTuple->arity = arity;
	prtType.type_union.nmTuple->fieldNames = (PRT_STRING *)PrtCalloc((size_t)arity, sizeof(PRT_STRING));
	prtType.type_union.nmTuple->fieldTypes = (PRT_TYPE *)PrtCalloc((size_t)arity, sizeof(PRT_TYPE));
	return prtType;
}

PRT_TYPE PrtMkTupType(_In_ PRT_UINT32 arity)
{
	PRT_TYPE prtType;
	PrtAssert(arity > 0, "Invalid tuple arity");
	prtType.type_union.tuple = (PRT_TUPTYPE *)PrtMalloc(sizeof(PRT_TUPTYPE));
	prtType.typeKind = PRT_KIND_TUPLE;
	prtType.type_union.tuple->arity = arity;
	prtType.type_union.tuple->fieldTypes = (PRT_TYPE *)PrtCalloc((size_t)arity, sizeof(PRT_TYPE));
	return prtType;
}

PRT_TYPE PrtMkSeqType(_In_ PRT_TYPE innerType)
{
	PRT_TYPE type;
	PrtAssert(innerType.typeKind >= 0 && innerType.typeKind < PRT_TYPE_KIND_COUNT, "Invalid type expression");
	type.type_union.seq = (PRT_SEQTYPE *)PrtMalloc(sizeof(PRT_SEQTYPE));
	type.typeKind = PRT_KIND_SEQ;
	type.type_union.seq->innerType = PrtCloneType(innerType);
	return type;
}

void PrtSetFieldType(_Inout_ PRT_TYPE tupleType, _In_ PRT_UINT32 index, _In_ PRT_TYPE fieldType)
{
	PrtAssert(tupleType.typeKind == PRT_KIND_TUPLE || tupleType.typeKind == PRT_KIND_NMDTUP, "Invalid type expression");
	PrtAssert(fieldType.typeKind >= 0 && fieldType.typeKind < PRT_TYPE_KIND_COUNT, "Invalid type expression");

	if (tupleType.typeKind == PRT_KIND_TUPLE)
	{
		
		PrtAssert(index < tupleType.type_union.tuple->arity, "Invalid tuple index");
		tupleType.type_union.tuple->fieldTypes[index] = PrtCloneType(fieldType);
	}
	else if (tupleType.typeKind == PRT_KIND_NMDTUP)
	{
		PrtAssert(index < tupleType.type_union.nmTuple->arity, "Invalid tuple index");
		tupleType.type_union.nmTuple->fieldTypes[index] = PrtCloneType(fieldType);
	}
}

void PrtSetFieldName(_Inout_ PRT_TYPE tupleType, _In_ PRT_UINT32 index, _In_ PRT_STRING fieldName)
{
	size_t nameLen;
	PRT_STRING fieldNameClone;
	PrtAssert(tupleType.typeKind == PRT_KIND_NMDTUP, "Invalid type expression");
	PrtAssert(fieldName != NULL && *fieldName != '\0', "Invalid field name");
	PrtAssert(index < tupleType.type_union.nmTuple->arity, "Invalid tuple index");
	nameLen = strnlen(fieldName, PRT_MAXFLDNAME_LENGTH);
	PrtAssert(nameLen > 0 && nameLen < PRT_MAXFLDNAME_LENGTH, "Invalid field name");
	fieldNameClone = (PRT_STRING)PrtCalloc(nameLen + 1, sizeof(PRT_CHAR));
	strncpy(fieldNameClone, fieldName, nameLen);
	fieldNameClone[nameLen] = '\0';
	tupleType.type_union.nmTuple->fieldNames[index] = fieldNameClone;
}

PRT_BOOLEAN PrtIsSubtype(_In_ PRT_TYPE subType, _In_ PRT_TYPE supType)
{
	PRT_TYPE_KIND subKind = subType.typeKind;
	PRT_TYPE_KIND supKind = supType.typeKind;
	switch (supKind)
	{
	case PRT_KIND_ANY:
		//// Everything is a subtype of `any`.
		return PRT_TRUE;
	case PRT_KIND_BOOL:
	case PRT_KIND_NULL:
	case PRT_KIND_EVENT:
	case PRT_KIND_MACHINE:
	case PRT_KIND_MODEL:
	{
		if (subKind == PRT_KIND_NULL)
			return PRT_TRUE;
	}
	case PRT_KIND_INT:
	case PRT_KIND_FORGN:
		//// These types do not have any proper subtypes.
		return subKind == supKind ? PRT_TRUE : PRT_FALSE;
	case PRT_KIND_MAP:
	{	
		//// Both types are maps and inner types are in subtype relationship.
		PRT_MAPTYPE *subMap;
		PRT_MAPTYPE *supMap;
		if (subKind != PRT_KIND_MAP)
		{
			return PRT_FALSE;
		}

		subMap = (PRT_MAPTYPE *)subType.type_union.map;
		supMap = (PRT_MAPTYPE *)supType.type_union.map;
		return
			PrtIsSubtype(subMap->domType, supMap->domType) &&
			PrtIsSubtype(subMap->codType, supMap->codType) ? PRT_TRUE : PRT_FALSE;
	}
	case PRT_KIND_NMDTUP:
	{
		//// Both types are named tuples with same field names, arity, and inner types are in subtype relationship.
		PRT_UINT32 i;
		PRT_NMDTUPTYPE *subNmdTup;
		PRT_NMDTUPTYPE *supNmdTup;
		if (subKind != PRT_KIND_NMDTUP)
		{
			return PRT_FALSE;
		}

		subNmdTup = (PRT_NMDTUPTYPE *)subType.type_union.nmTuple;
		supNmdTup = (PRT_NMDTUPTYPE *)supType.type_union.nmTuple;
		if (subNmdTup->arity != supNmdTup->arity)
		{
			return PRT_FALSE;
		}
		
		//// Next check field names.
		for (i = 0; i < subNmdTup->arity; ++i)
		{
			if (strncmp(subNmdTup->fieldNames[i], supNmdTup->fieldNames[i], PRT_MAXFLDNAME_LENGTH) != 0)
			{
				return PRT_FALSE;
			}
		}

		//// Finally check field types.
		for (i = 0; i < subNmdTup->arity; ++i)
		{
			if (!PrtIsSubtype(subNmdTup->fieldTypes[i], supNmdTup->fieldTypes[i]))
			{
				return PRT_FALSE;
			}
		}

		return PRT_TRUE;
	}
	case PRT_KIND_SEQ:
	{
		//// Both types are sequences and inner types are in subtype relationship.
		PRT_SEQTYPE *subSeq;
		PRT_SEQTYPE *supSeq;
		if (subKind != PRT_KIND_SEQ)
		{
			return PRT_FALSE;
		}

		subSeq = (PRT_SEQTYPE *)subType.type_union.seq;
		supSeq = (PRT_SEQTYPE *)supType.type_union.seq;
		return PrtIsSubtype(subSeq->innerType, supSeq->innerType);
	}
	case PRT_KIND_TUPLE:
	{
		//// Both types are tuples with same arity, and inner types are in subtype relationship.
		PRT_UINT32 i;
		PRT_TUPTYPE *subTup;
		PRT_TUPTYPE *supTup;
		if (subKind != PRT_KIND_TUPLE)
		{
			return PRT_FALSE;
		}

		subTup = (PRT_TUPTYPE *)subType.type_union.tuple;
		supTup = (PRT_TUPTYPE *)supType.type_union.tuple;
		if (subTup->arity != supTup->arity)
		{
			return PRT_FALSE;
		}

		//// Finally check field types.
		for (i = 0; i < subTup->arity; ++i)
		{
			if (!PrtIsSubtype(subTup->fieldTypes[i], supTup->fieldTypes[i]))
			{
				return PRT_FALSE;
			}
		}

		return PRT_TRUE;
	}
	default:
		PrtAssert(PRT_FALSE, "Invalid type");
		return PRT_FALSE;
	}
}

PRT_TYPE PrtCloneType(_In_ PRT_TYPE type)
{
	PRT_TYPE_KIND kind = type.typeKind;
	switch (kind)
	{
	case PRT_KIND_ANY:
	case PRT_KIND_BOOL:
	case PRT_KIND_EVENT:
	case PRT_KIND_MACHINE:
	case PRT_KIND_INT:
	case PRT_KIND_MODEL:
	case PRT_KIND_NULL:
		return PrtMkPrimitiveType(kind);
	case PRT_KIND_FORGN:
	{
		PRT_FORGNTYPE *ftype = type.type_union.forgn;
		
		return PrtMkForgnType(ftype->typeTag, ftype->cloner, ftype->freer, ftype->hasher, ftype->eqTester);
	}
	case PRT_KIND_MAP:
	{		
		PRT_MAPTYPE *mtype = type.type_union.map;
		return PrtMkMapType(mtype->domType, mtype->codType);
	}
	case PRT_KIND_NMDTUP:
	{
		PRT_UINT32 i;
		PRT_NMDTUPTYPE *ntype = type.type_union.nmTuple;
		PRT_TYPE clone = PrtMkNmdTupType(ntype->arity);
		for (i = 0; i < ntype->arity; ++i)
		{
			PrtSetFieldName(clone, i, ntype->fieldNames[i]);
			PrtSetFieldType(clone, i, ntype->fieldTypes[i]);
		}

		return clone;
	}
	case PRT_KIND_SEQ:
	{
		PRT_SEQTYPE *stype = type.type_union.seq;
		return PrtMkSeqType(stype->innerType);
	}
	case PRT_KIND_TUPLE:
	{
		PRT_UINT32 i;
		PRT_TUPTYPE *ttype = type.type_union.tuple;
		PRT_TYPE clone = PrtMkTupType(ttype->arity);
		for (i = 0; i < ttype->arity; ++i)
		{
			PrtSetFieldType(clone, i, ttype->fieldTypes[i]);
		}

		return clone;
	}
	default:
		PrtAssert(PRT_FALSE, "Invalid type");
		PRT_TYPE rettype;
		rettype.typeKind = PRT_TYPE_KIND_CANARY;
		return rettype;
	}
}

void PrtFreeType(_Inout_ PRT_TYPE type)
{
	PRT_TYPE_KIND kind = type.typeKind;
	switch (kind)
	{
	case PRT_KIND_ANY:
	case PRT_KIND_BOOL:
	case PRT_KIND_EVENT:
	case PRT_KIND_MACHINE:
	case PRT_KIND_INT:
	case PRT_KIND_MODEL:
	case PRT_KIND_NULL:
		type.typeKind = PRT_TYPE_KIND_CANARY;
		break;
	case PRT_KIND_FORGN:
	{
		PRT_FORGNTYPE *ftype = type.type_union.forgn;
		type.typeKind = PRT_TYPE_KIND_CANARY;
		PrtFree(ftype);
		break;
	}
	case PRT_KIND_MAP:
	{
		PRT_MAPTYPE *mtype = (PRT_MAPTYPE *)type.type_union.map;
		PrtFreeType(mtype->domType);
		PrtFreeType(mtype->codType);
		type.typeKind = PRT_TYPE_KIND_CANARY;
		PrtFree(mtype);
		break;
	}
	case PRT_KIND_NMDTUP:
	{
		PRT_UINT32 i;
		PRT_NMDTUPTYPE *ntype = type.type_union.nmTuple;
		for (i = 0; i < ntype->arity; ++i)
		{
			PrtFree(ntype->fieldNames[i]);
			PrtFreeType(ntype->fieldTypes[i]);
		}

		PrtFree(ntype->fieldNames);
		PrtFree(ntype->fieldTypes);
		type.typeKind = PRT_TYPE_KIND_CANARY;
		PrtFree(ntype);
		break;
	}
	case PRT_KIND_SEQ:
	{
		PRT_SEQTYPE *stype = type.type_union.seq;
		PrtFreeType(stype->innerType);
		type.typeKind = PRT_TYPE_KIND_CANARY;
		PrtFree(stype);
		break;
	}
	case PRT_KIND_TUPLE:
	{
		PRT_UINT32 i;
		PRT_TUPTYPE *ttype = type.type_union.tuple;
		for (i = 0; i < ttype->arity; ++i)
		{
			PrtFreeType(ttype->fieldTypes[i]);
		}

		PrtFree(ttype->fieldTypes);
		type.typeKind = PRT_TYPE_KIND_CANARY;
		PrtFree(ttype);
		break;
	}
	default:
		PrtAssert(PRT_FALSE, "Invalid type");
		break;
	}
}

/** The "Absent type" is a built-in foreign type used to represent the absence of a foreign value.
* The type tag of the absent type 0. No other foreign type is permitted to use this type tag.
* @param[in] typeTag A type tag.
* @returns `true` if the typeTag is 0, `false` otherwise.
*/
PRT_BOOLEAN PrtIsAbsentTag(_In_ PRT_GUID typeTag)
{
	return 
		typeTag.data1 == 0 &&
		typeTag.data2 == 0 &&
		typeTag.data3 == 0 &&
		typeTag.data4 == 0 ? PRT_TRUE : PRT_FALSE;
}

/** The "Absent type" is a built-in foreign type used to represent the absence of a foreign value.
* The absent type has a single value, which is NULL.
* @param[in] typeTag The type tag of the absent type is always 0.
* @param[in[ frgnVal The frgnVal must be NULL.
* @returns NULL.
*/
void *PrtAbsentTypeClone(_In_ PRT_GUID typeTag, _In_ void *frgnVal)
{
	PrtAssert(PrtIsAbsentTag(typeTag), "Expected the absent type");
	PrtAssert(frgnVal != NULL, "Expected the absent value");
	return NULL;
}

/** The "Absent type" is a built-in foreign type used to represent the absence of a foreign value.
* The absent type has a single value, which is NULL. Does nothing.
* @param[in] typeTag The type tag of the absent type is always 0.
* @param[in[ frgnVal The frgnVal must be NULL.
*/
void PrtAbsentTypeFree(_In_ PRT_GUID typeTag, _Inout_ void *frgnVal)
{
	PrtAssert(PrtIsAbsentTag(typeTag), "Expected the absent type");
	PrtAssert(frgnVal != NULL, "Expected the absent value");
}

/** The "Absent type" is a built-in foreign type used to represent the absence of a foreign value.
* The absent type has a single value, which is NULL.
* @param[in] typeTag The type tag of the absent type is always 0.
* @param[in[ frgnVal The frgnVal must be NULL.
* @returns 0.
*/
PRT_UINT32 PrtAbsentTypeGetHashCode(_In_ PRT_GUID typeTag, _In_ void *frgnVal)
{
	PrtAssert(PrtIsAbsentTag(typeTag), "Expected the absent type");
	PrtAssert(frgnVal != NULL, "Expected the absent value");
	return 0;
}

/** The "Absent type" is a built-in foreign type used to represent the absence of a foreign value.
* The absent type has a single value, which is NULL. One of the inputs must be `NULL : Absent`.
* @param[in] typeTag1 The type tag of the first foreign value.
* @param[in] frgnVal1 A pointer to the first foreign data.
* @param[in] typeTag2 The type tag of the second foreign value.
* @param[in] frgnVal2 A pointer to the second foreign data.
* @returns `true` if both inputs are absent, `false` otherwise.
*/
PRT_BOOLEAN PrtAbsentTypeIsEqual(
	_In_ PRT_GUID typeTag1,
	_In_ void *frgnVal1,
	_In_ PRT_GUID typeTag2,
	_In_ void *frgnVal2)
{
	PrtAssert(PrtIsAbsentTag(typeTag1) || PrtIsAbsentTag(typeTag2), "Expected an absent value");
	PrtAssert(!PrtIsAbsentTag(typeTag1) || frgnVal1 == NULL, "Invalid absent value");
	PrtAssert(!PrtIsAbsentTag(typeTag2) || frgnVal2 == NULL, "Invalid absent value");
	if (PrtIsAbsentTag(typeTag1))
	{
		return PrtIsAbsentTag(typeTag2) ? PRT_TRUE : PRT_FALSE;
	}
	else if (PrtIsAbsentTag(typeTag2))
	{
		return PrtIsAbsentTag(typeTag1) ? PRT_TRUE : PRT_FALSE;
	}

	return PRT_FALSE;
}

PRT_TYPE PrtMkAbsentType()
{
	PRT_TYPE abstType;
	abstType.type_union.forgn = (PRT_FORGNTYPE *)PrtMalloc(sizeof(PRT_FORGNTYPE));
	abstType.typeKind = PRT_KIND_FORGN;
	abstType.type_union.forgn->typeTag.data1 = 0;
	abstType.type_union.forgn->typeTag.data2 = 0;
	abstType.type_union.forgn->typeTag.data3 = 0;
	abstType.type_union.forgn->typeTag.data4 = 0;
	abstType.type_union.forgn->cloner = &PrtAbsentTypeClone;
	abstType.type_union.forgn->freer = &PrtAbsentTypeFree;
	abstType.type_union.forgn->hasher = &PrtAbsentTypeGetHashCode;
	abstType.type_union.forgn->eqTester = &PrtAbsentTypeIsEqual;

	return abstType;
}
